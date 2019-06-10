using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Wikiled.Common.Logging;
using Wikiled.Common.Utilities.Auth;
using Wikiled.Console.Auth;
using Wikiled.FreeAgent.Auth;
using Wikiled.FreeAgent.Client;

namespace Wikiled.FreeAgent.TestApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            NLog.LogManager.LoadConfiguration("nlog.config");
            ApplicationLogging.LoggerFactory.AddNLog();
            //FreeAgentClient.UseSandbox = true;
            var client = new FreeAgentClient(new Logger<FreeAgentClient>(ApplicationLogging.LoggerFactory),
                                             KeyStorage.AppKey,
                                             KeyStorage.AppSecret);
            var helper = new OAuthHelper(new Logger<OAuthHelper>(ApplicationLogging.LoggerFactory));
            helper.RedirectUri = "http://localhost:9000";
            var auth =
                new PersistedAuthentication<AccessTokenData>(
                    new Logger<PersistedAuthentication<AccessTokenData>>(ApplicationLogging.LoggerFactory),
                    new ConsoleOAuthAuthentication<AccessTokenData>(client, helper));
            await auth.Authenticate().ConfigureAwait(false);
            
            var banks = await client.BankAccount.All().ToArray();
            var transactions = await client.BankTransactionExplanation.All(banks[3]).Where(item => item.Attachment != null).Take(10).ToArray();

            var request = WebRequest.Create(new Uri(transactions[0].Attachment.ContentSrc));
            using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (var file = new FileStream("file.pdf", FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(file);
                    }
                }
            }
        }
    }
}
