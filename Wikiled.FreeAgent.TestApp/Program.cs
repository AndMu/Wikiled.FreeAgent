using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Collections.Generic;
using System.Reactive.Linq;
using Wikiled.Common.Logging;
using Wikiled.Common.Utilities.Auth;
using Wikiled.Console.Auth;
using Wikiled.FreeAgent.Auth;
using Wikiled.FreeAgent.Client;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.TestApp
{
    public class Program
    {
        public static async System.Threading.Tasks.Task Main(string[] args)
        {
            NLog.LogManager.LoadConfiguration("nlog.config");
            ApplicationLogging.LoggerFactory.AddNLog();
            //FreeAgentClient.UseSandbox = true;
            var client = new FreeAgentClient(new Logger<FreeAgentClient>(ApplicationLogging.LoggerFactory),
                                             KeyStorage.AppKey,
                                             KeyStorage.AppSecret);
            var helper = new OAuthHelper(new Logger<OAuthHelper>(ApplicationLogging.LoggerFactory));
            var auth =
                new PersistedAuthentication<AccessTokenData>(
                    new Logger<PersistedAuthentication<AccessTokenData>>(ApplicationLogging.LoggerFactory),
                    new ConsoleOAuthAuthentication<AccessTokenData>(client, helper));
            await auth.Authenticate().ConfigureAwait(false);
            //var timeline = await client.Company.TaxTimeline().ConfigureAwait(false);
            var banks = await client.BankAccount.All().ToArray();
            var transactions = await client.BankTransactionExplanation.All(banks[0].name).ToArray();
        }
    }
}
