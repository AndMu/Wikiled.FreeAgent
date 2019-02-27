using System;
using System.Net;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Serialization.Json;
using Wikiled.FreeAgent.Exceptions;
using Wikiled.FreeAgent.Extensions;
using Wikiled.FreeAgent.Helpers;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Client
{
    public class FreeAgentClient
    {
        private readonly ILogger<FreeAgentClient> logger;
      
        public AccessTokenData RefreshAccessToken()
        {
            restClient.BaseUrl = new Uri(BaseUrl);

            var request = Helper.CreateRefreshTokenRequest();
            var response = Execute<AccessTokenData>(request);

            if (response != null && !string.IsNullOrEmpty(response.AccessToken))
            {
                var token = CurrentAccessToken;
                token.AccessToken = response.AccessToken;
                token.ExpiresIn = response.ExpiresIn;

                CurrentAccessToken = token;
                return CurrentAccessToken;
            }

            return null;
        }

        public const string Version = "2";

        private const string ApiBaseUrl = "https://api.freeagent.com";

        private const string ApiSandboxBaseUrl = "https://api.sandbox.freeagent.com";

        public static WebProxy Proxy = null;

        private readonly string apiKey;

        private readonly string appsecret;

        public AccountingClient Accounting;

        public BankAccountClient BankAccount;

        public BankTransactionClient BankTransaction;

        public BankTransactionExplanationClient BankTransactionExplanation;

        public BillClient Bill;

        public CategoryClient Categories;

        public CompanyClient Company;

        public ContactClient Contact;

        public ExpenseClient Expense;

        public InvoiceClient Invoice;

        public ProjectClient Project;

        public TaskClient Task;

        public TimeslipClient Timeslip;

        public UserClient User;

        private AccessTokenData currentAccessToken;

        private RestClient restClient;

        private RestClient restClientModified;


        public FreeAgentClient(ILogger<FreeAgentClient> logger, string apiKey, string appSecret, AccessTokenData savedToken = null)
        {
            this.apiKey = apiKey;
            appsecret = appSecret;
            CurrentAccessToken = savedToken;
            this.logger = logger;

            LoadClient();
        }

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

        public string BuildAuthorizeUrl(string callback = null)
        {
            //Go 1-Liner!
            return $"{BaseUrl}/v{Version}/approve_app?response_type=code&client_id={apiKey}{(string.IsNullOrEmpty(callback) ? string.Empty : "&redirect_uri=" + callback.UrlEncode())}&state=foo";
        }

        public AccessTokenData GetAccessToken(string code, string redirectUri = "")
        {
            restClient.BaseUrl = new Uri(BaseUrl);
            var request = Helper.CreateAccessTokenRequest(code, redirectUri);
            var response = Execute<AccessTokenData>(request);
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
            int val = (int)code;
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
            Helper.ApiKey = apiKey;
            Helper.ApiSecret = appsecret;

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

        internal T Execute<T>(IRestRequest request) where T : new()
        {
            SetProxy();
            var response = restClient.Execute<T>(request);
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
