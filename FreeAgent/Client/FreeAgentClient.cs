using System;
using System.Net;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Serialization.Json;
using Wikiled.FreeAgent.Exceptions;
using Wikiled.FreeAgent.Extensions;
using Wikiled.FreeAgent.Helpers;
using Wikiled.FreeAgent.Models;
using RestClientExtensions = Wikiled.FreeAgent.Extensions.RestClientExtensions;

namespace Wikiled.FreeAgent.Client
{
    public class FreeAgentClient
    {
        /// <summary>
        ///     Gets a token from the almightly dropbox.com (Token cant be used until authorized!)
        /// </summary>
        /// <returns></returns>
        public AccessToken GetAccessToken(string code, string redirectUri = "")
        {
            restClient.BaseUrl = new Uri(BaseUrl);
            var request = Helper.CreateAccessTokenRequest(code, redirectUri);
            var response = Execute<AccessToken>(request);

            if (response != null && !string.IsNullOrEmpty(response.access_token))
            {
                CurrentAccessToken = response;
            }

            return CurrentAccessToken;
        }

        public AccessToken RefreshAccessToken()
        {
            restClient.BaseUrl = new Uri(BaseUrl);

            var request = Helper.CreateRefreshTokenRequest();
            var response = Execute<AccessToken>(request);

            if (response != null && !string.IsNullOrEmpty(response.access_token))
            {
                var token = CurrentAccessToken;

                token.access_token = response.access_token;
                token.expires_in = response.expires_in;

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

        /// <summary>
        ///     To use Dropbox API in sandbox mode (app folder access) set to true
        /// </summary>
        private static bool useSandbox;

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

        private AccessToken currentAccessToken;

        private RestClient restClient;

        private RestClient restClientModified;

        /// <summary>
        ///     Default Constructor for the DropboxClient
        /// </summary>
        /// <param name="apiKey">The Api Key to use for the Dropbox Requests</param>
        /// <param name="appSecret">The Api Secret to use for the Dropbox Requests</param>
        public FreeAgentClient(string apiKey, string appSecret)
        {
            this.apiKey = apiKey;
            appsecret = appSecret;
            LoadClient();
        }

        /// <summary>
        ///     Creates an instance of the DropNetClient given an API Key/Secret and a User Token/Secret
        /// </summary>
        /// <param name="apiKey">The Api Key to use for the Dropbox Requests</param>
        /// <param name="appSecret">The Api Secret to use for the Dropbox Requests</param>
        /// <param name="userToken">The User authentication token</param>
        /// <param name="userSecret">The Users matching secret</param>
        public FreeAgentClient(string apiKey, string appSecret, AccessToken savedToken)
        {
            this.apiKey = apiKey;
            appsecret = appSecret;
            CurrentAccessToken = savedToken;

            LoadClient();
        }

        public static bool UseSandbox
        {
            get => useSandbox;
            set => useSandbox = value;
        }

        public AccessToken CurrentAccessToken
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

        /// <summary>
        ///     Helper Method to Build up the Url to authorize a Token/Secret
        /// </summary>
        /// <param name="userLogin"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public string BuildAuthorizeUrl(string callback = null)
        {
            //Go 1-Liner!
            return $"{BaseUrl}/v{Version}/approve_app?response_type=code&client_id={apiKey}{(string.IsNullOrEmpty(callback) ? string.Empty : "&redirect_uri=" + callback.UrlEncode())}&state=foo";
        }

        public string ExtractCodeFromUrl(string url)
        {
            //url will contain code=.... in the parameters

            Uri uri = new Uri(url);
            string query = uri.Query;
            if (string.IsNullOrEmpty(query) || !query.Contains("code="))
            {
                return "";
            }

            var elements = query.ParseQueryString();

            if (elements.ContainsKey("code"))
                return elements["code"];

            return "";
        }

        public void SetProxy()
        {
            restClient.Proxy = Proxy;
        }

        protected void ExecuteAsync(IRestRequest request, Action<IRestResponse> success, Action<FreeAgentException> failure)
        {
            restClient.ExecuteAsync(
                request,
                (response, asynchandler) =>
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        failure(new FreeAgentException(response));
                    }
                    else
                    {
                        success(response);
                    }
                });
        }

        protected void ExecuteAsync<T>(IRestRequest request, Action<T> success, Action<FreeAgentException> failure) where T : new()
        {
            restClient.ExecuteAsync<T>(
                request,
                (response, asynchandler) =>
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        failure(new FreeAgentException(response));
                    }
                    else
                    {
                        success(response.Data);
                    }
                });
        }

        private Task<T> ExecuteTask<T>(IRestRequest request) where T : new()
        {
            return RestClientExtensions.ExecuteTask<T>(restClient, request);
        }

        private Task<IRestResponse> ExecuteTask(IRestRequest request)
        {
            return RestClientExtensions.ExecuteTask(restClient, request);
        }

        private bool IsSuccess(HttpStatusCode code)
        {
            int val = (int)code;
            if (val < 299)
                return true;
            return false;
        }

        private void LoadClient()
        {
            restClient = new RestClient(BaseUrl);
            restClient.ClearHandlers();
            restClient.AddHandler("application/json", new JsonDeserializer());

            Helper = new RequestHelper(Version);
            Helper.ApiKey = apiKey;
            Helper.ApiSecret = appsecret;

            SetProxy();

            //Default to full access
            //UseSandbox = false;
            SetupClients();
        }

        private void SetAuthentication(RestRequest request)
        {
            request.AddHeader("Authorization", "Bearer " + CurrentAccessToken.access_token);
        }

        private void SetAuthProviders()
        {
            //if (UserLogin != null)
            //{
            //Set the OauthAuthenticator only when the UserLogin property changes
            //   _restClient.Authenticator = new OAuthAuthenticator(_restClient.BaseUrl, _apiKey, _appsecret, UserLogin.Token, UserLogin.Secret);
            //}
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
                Console.WriteLine(response.Content);
                throw new FreeAgentException(response);
            }

            if (response.Data == null)
            {
                Console.WriteLine("{0} returned null", restClient.BuildUri(request));
            }

            return response.Data;
        }

        internal IRestResponse Execute(IRestRequest request)
        {
            IRestResponse response;

            response = restClient.Execute(request);

            if (!IsSuccess(response.StatusCode))
            {
                throw new FreeAgentException(response);
            }

            return response;
        }
    }
}
