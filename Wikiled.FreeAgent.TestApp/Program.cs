using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Wikiled.FreeAgent.Client;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.TestApp
{
    public class Program
    {
        public static async System.Threading.Tasks.Task Main(string[] args)
        {
            FreeAgentClient.UseSandbox = true;
            var client = new FreeAgentClient(KeyStorage.AppKey, KeyStorage.AppSecret);
            var helper = new OAuthHelper(new Logger<OAuthHelper>(new LoggerFactory()));
            helper.StartService();
            var auth = client.BuildAuthorizeUrl(helper.RedirectUri);
            await helper.Start(auth).ConfigureAwait(false);
            var newToken = client.GetAccessToken(helper.Code, null);
            //client.BankTransactionExplanation.All();
            List<TaxTimeline> timeline = client.Company.TaxTimeline();
        }
    }
}
