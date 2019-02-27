using System;
using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using Wikiled.FreeAgent.Client;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Tests
{
    public class BaseFixture
    {
        protected FreeAgentClient Client;

        protected AccessTokenData Token;

        public virtual void Configure()
        {
        }

        public virtual void SetupClient()
        {
            Configure();

            FreeAgentClient.UseSandbox = KeyStorage.UseSandbox;
            if (KeyStorage.UseProxy) FreeAgentClient.Proxy = new WebProxy("127.0.0.1", 8888);

            Client = new FreeAgentClient(new NullLogger<FreeAgentClient>(), KeyStorage.AppKey, KeyStorage.AppSecret);

            var sandbox_bttest_token = new AccessTokenData
                                       {
                                           AccessToken = "",
                                           RefreshToken = KeyStorage.RefreshToken,
                                           TokenType = "bearer"
                                       };

            Client.CurrentAccessToken = sandbox_bttest_token;

            Token = Client.RefreshAccessToken();

            if (Token == null || string.IsNullOrEmpty(Token.AccessToken) || string.IsNullOrEmpty(Token.RefreshToken))
            {
                throw new Exception("Could not setup the Token");
            }
        }
    }
}
