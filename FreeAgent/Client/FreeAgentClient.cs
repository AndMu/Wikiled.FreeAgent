using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Serialization.Json;
using Wikiled.Common.Utilities.Auth;
using Wikiled.FreeAgent.Auth;
using Wikiled.FreeAgent.Exceptions;
using Wikiled.FreeAgent.Extensions;
using Wikiled.FreeAgent.Helpers;

namespace Wikiled.FreeAgent.Client
{
    public class FreeAgentClient : IAuthClient<AccessTokenData>, IFreeAgentClient
    {
        public const string Version = "2";

        private const string ApiBaseUrl = "https://api.freeagent.com";

        private const string ApiSandboxBaseUrl = "https://api.sandbox.freeagent.com";

        public static WebProxy Proxy = null;

        private readonly ILogger<FreeAgentClient> logger;

        private readonly AuthenticationData auth;

        private AccessTokenData currentAccessToken;

        private RestClient restClient;

        public FreeAgentClient(ILogger<FreeAgentClient> logger, AuthenticationData auth)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.auth = auth ?? throw new ArgumentNullException(nameof(auth));
            LoadClient();
        }

        public AccountingClient Accounting { get; set; }

        public BankAccountClient BankAccount { get; set; }

        public BankTransactionClient BankTransaction { get; set; }

        public BankTransactionExplanationClient BankTransactionExplanation { get; set; }

        public BillClient Bill { get; set; }

        public CategoryClient Categories { get; set; }

        public CompanyClient Company { get; set; }

        public ContactClient Contact { get; set; }

        public ExpenseClient Expense { get; set; }

        public InvoiceClient Invoice { get; set; }

        public ProjectClient Project { get; set; }

        public TaskClient Task { get; set; }

        public TimeslipClient Timeslip { get; set; }

        public UserClient User { get; private set; }

        public static bool UseSandbox { get; set; }

        public AccessTokenData CurrentAccessToken
        {
            get => currentAccessToken;
            set
            {
                currentAccessToken = value;
                if (Helper != null)
                {
                    Helper.CurrentAccessToken = currentAccessToken;
                }
            }
        }

        public RequestHelper Helper { get; private set; }

        /// <summary>
        ///     Gets the directory root for the requests (full or sandbox mode)
        /// </summary>
        private string BaseUrl => UseSandbox ? ApiSandboxBaseUrl : ApiBaseUrl;

        public async Task<AccessTokenData> RefreshToken(AccessTokenData token)
        {
            restClient.BaseUrl = new Uri(BaseUrl);

            var request = Helper.CreateRefreshTokenRequest(token);
            var response = await Execute<AccessTokenData>(request).ConfigureAwait(false);
            CurrentAccessToken = null;
            if (response != null && !string.IsNullOrEmpty(response.AccessToken))
            {
                token.AccessToken = response.AccessToken;
                token.ExpiresIn = response.ExpiresIn;

                CurrentAccessToken = token;
                return CurrentAccessToken;
            }

            return null;
        }

        public string BuildAuthorizeUrl(string callback = null)
        {
            //Go 1-Liner!
            return
                $"{BaseUrl}/v{Version}/approve_app?response_type=code&client_id={auth.ApiKey}{(string.IsNullOrEmpty(callback) ? string.Empty : "&redirect_uri=" + callback.UrlEncode())}&state=foo";
        }

        public async Task<AccessTokenData> GetToken(string code, string redirectUri = null)
        {
            restClient.BaseUrl = new Uri(BaseUrl);
            var request = Helper.CreateAccessTokenRequest(code, redirectUri);
            var response = await Execute<AccessTokenData>(request).ConfigureAwait(false);
            if (response != null && !string.IsNullOrEmpty(response.AccessToken))
            {
                CurrentAccessToken = response;
            }

            return CurrentAccessToken;
        }

        public void SetProxy()
        {
            restClient.Proxy = Proxy;
        }

        private bool IsSuccess(HttpStatusCode code)
        {
            var val = (int)code;
            if (val < 299)
            {
                return true;
            }

            return false;
        }

        private void LoadClient()
        {
            restClient = new RestClient(BaseUrl);
            restClient.ClearHandlers();
            restClient.AddHandler("application/json", () => new JsonDeserializer());
            Helper = new RequestHelper(Version);
            Helper.ApiKey = auth.ApiKey;
            Helper.ApiSecret = auth.AppSecret;
            SetProxy();
            SetupClients();
        }

        private void SetAuthentication(RestRequest request)
        {
            request.AddHeader("Authorization", "Bearer " + CurrentAccessToken.AccessToken);
        }

        private void SetupClients()
        {
            Company = new CompanyClient(this);
            Accounting = new AccountingClient(this);
            Contact = new ContactClient(this);
            Project = new ProjectClient(this);
            Expense = new ExpenseClient(this);
            Invoice = new InvoiceClient(this);
            Task = new TaskClient(this);
            Timeslip = new TimeslipClient(this);
            User = new UserClient(this);
            BankAccount = new BankAccountClient(this);
            Categories = new CategoryClient(this);
            BankTransaction = new BankTransactionClient(this);
            BankTransactionExplanation = new BankTransactionExplanationClient(this);
            Bill = new BillClient(this);
        }

        internal async Task<T> Execute<T>(IRestRequest request) where T : new()
        {
            SetProxy();
            var response = await restClient.ExecuteTaskAsync<T>(request).ConfigureAwait(false);
            if (!IsSuccess(response.StatusCode))
            {
                logger.LogError(response.Content);
                throw new FreeAgentException(response);
            }

            if (response.Data == null)
            {
                logger.LogError("{0} returned null", restClient.BuildUri(request));
            }

            return response.Data;
        }

        internal IRestResponse Execute(IRestRequest request)
        {
            var response = restClient.Execute(request);
            if (!IsSuccess(response.StatusCode))
            {
                throw new FreeAgentException(response);
            }

            return response;
        }
    }
}