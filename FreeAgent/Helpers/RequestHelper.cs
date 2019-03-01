using RestSharp;
using Wikiled.FreeAgent.Auth;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Helpers
{
    /// <summary>
    ///     Helper class for creating DropNet RestSharp Requests
    /// </summary>
    public class RequestHelper
    {
        private readonly string version;

        public RequestHelper(string version)
        {
            this.version = version;
        }

        public string ApiKey { private get; set; }

        public string ApiSecret { private get; set; }

        public AccessTokenData CurrentAccessToken { get; set; }

        public RestRequest CreateAccessTokenRequest(string code, string callback = null)
        {
            // grant_type=authorization_code
            // code=<the code from the url>
            // redirect_url = <if it was provided before>

            var request = new RestRequest(Method.POST);
            request.Resource = "v{version}/token_endpoint";
            request.AddParameter("version", version, ParameterType.UrlSegment);
            request.AddParameter("code", code, ParameterType.GetOrPost);
            request.AddParameter("grant_type", "authorization_code", ParameterType.GetOrPost);
            request.AddParameter("client_id", ApiKey, ParameterType.GetOrPost);
            request.AddParameter("client_secret", ApiSecret, ParameterType.GetOrPost);
            if (!string.IsNullOrEmpty(callback))
            {
                request.AddParameter("redirect_uri", callback, ParameterType.GetOrPost);
            }

            return request;
        }

        public RestRequest CreateRefreshTokenRequest()
        {
            // grant_type=authorization_code
            // code=<the code from the url>
            // redirect_url = <if it was provided before>

            var request = new RestRequest(Method.POST);
            request.Resource = "v{version}/token_endpoint";
            request.AddParameter("version", version, ParameterType.UrlSegment);
            request.AddParameter("grant_type", "refresh_token", ParameterType.GetOrPost);
            request.AddParameter("client_id", ApiKey, ParameterType.GetOrPost);
            request.AddParameter("client_secret", ApiSecret, ParameterType.GetOrPost);
            request.AddParameter("refresh_token", CurrentAccessToken.RefreshToken, ParameterType.GetOrPost);

            return request;
        }
    }
}
