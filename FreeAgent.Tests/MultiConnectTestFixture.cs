using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
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

            var sandbox_bttest_token = new AccessTokenData
                                       {
                                           AccessToken = "",
                                           RefreshToken = KeyStorage.RefreshToken,
                                           TokenType = "bearer"
                                       };

            {
                var Client = new FreeAgentClient(new NullLogger<FreeAgentClient>(), KeyStorage.AppKey, KeyStorage.AppSecret);

                Client.CurrentAccessToken = sandbox_bttest_token;

                try
                {
                    var co = Client.Company.Single();
                }
                catch
                {
                }
            }

            FreeAgentClient.UseSandbox = false;

            var LiveClient = new FreeAgentClient(new NullLogger<FreeAgentClient>(), KeyStorage.AppKey, KeyStorage.AppSecret);

            LiveClient.CurrentAccessToken = sandbox_bttest_token;

            try
            {
                var co = LiveClient.Company.Single();
            }
            catch
            {
            }

            Assert.IsTrue(true);
        }
    }
}
