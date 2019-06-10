namespace Wikiled.FreeAgent.Auth
{
    public class AuthenticationData
    {
        public AuthenticationData(string apiKey, string appSecret, AccessTokenData savedToken)
        {
            ApiKey = apiKey;
            AppSecret = appSecret;
            SavedToken = savedToken;
        }

        public string ApiKey { get; }

        public string AppSecret { get; }

        public AccessTokenData SavedToken { get; }
    }
}
