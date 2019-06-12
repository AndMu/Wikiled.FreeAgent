using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Wikiled.FreeAgent.Auth;
using Wikiled.FreeAgent.Client;

namespace Wikiled.FreeAgent.Tests
{
    public class BaseFixture
    {
        protected FreeAgentClient Client;

        protected AccessTokenData Token;

        public virtual void Configure()
        {
        }

        public virtual async Task SetupClient()
        {
            Configure();

            FreeAgentClient.UseSandbox = KeyStorage.UseSandbox;
            if (KeyStorage.UseProxy)
            {
                FreeAgentClient.Proxy = new WebProxy("127.0.0.1", 8888);
            }

            Client = new FreeAgentClient(new NullLogger<FreeAgentClient>(), new AuthenticationData(KeyStorage.AppKey, KeyStorage.AppSecret));

            var sandboxTestToken = new AccessTokenData
                                       {
                                           AccessToken = "",
                                           RefreshToken = KeyStorage.RefreshToken,
                                           TokenType = "bearer"
                                       };

            Client.CurrentAccessToken = sandboxTestToken;

            Token = await Client.RefreshToken(sandboxTestToken).ConfigureAwait(false);

            if (Token == null || string.IsNullOrEmpty(Token.AccessToken) || string.IsNullOrEmpty(Token.RefreshToken))
            {
                throw new Exception("Could not setup the Token");
            }
        }
    }
}
