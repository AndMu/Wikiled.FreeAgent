using System;

namespace Wikiled.FreeAgent.Auth
{
    public class AuthenticationData
    {
        public AuthenticationData(string apiKey, string appSecret)
        {
            ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            AppSecret = appSecret ?? throw new ArgumentNullException(nameof(appSecret));
        }

        public string ApiKey { get; }

        public string AppSecret { get; }
    }
}
