using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Wikiled.FreeAgent.Auth;
using Wikiled.FreeAgent.Client;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Tests
{
    public class MultiConnectTestFixture
    {
        protected FreeAgentClient Client, LiveClient;

        protected AccessTokenData Token;

        public virtual void Configure()
        {
        }

        [SetUp]
        public virtual void SetupClient()
        {
            Configure();
        }

        [Test]
        public void CanGetList()
        {
            FreeAgentClient.UseSandbox = true;

            var sandboxBttestToken = new AccessTokenData
            {
                AccessToken = "",
                RefreshToken = KeyStorage.RefreshToken,
                TokenType = "bearer"
            };

            var client = new FreeAgentClient(new NullLogger<FreeAgentClient>(), new AuthenticationData(KeyStorage.AppKey, KeyStorage.AppSecret));
            client.CurrentAccessToken = sandboxBttestToken;

            try
            {
                var co = client.Company.Single();
            }
            catch
            {
            }

            FreeAgentClient.UseSandbox = false;
            var liveClient = new FreeAgentClient(new NullLogger<FreeAgentClient>(), new AuthenticationData(KeyStorage.AppKey, KeyStorage.AppSecret));
            liveClient.CurrentAccessToken = sandboxBttestToken;

            try
            {
                var co = liveClient.Company.Single();
            }
            catch
            {
            }

            Assert.IsTrue(true);
        }
    }
}
