using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Collections.Generic;
using Wikiled.Common.Logging;
using Wikiled.Console.Auth;
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
            FreeAgentClient.UseSandbox = true;
            var client = new FreeAgentClient(new Logger<FreeAgentClient>(ApplicationLogging.LoggerFactory),
                                             KeyStorage.AppKey,
                                             KeyStorage.AppSecret);
            var helper = new OAuthHelper(new Logger<OAuthHelper>(ApplicationLogging.LoggerFactory));
            var auth = client.BuildAuthorizeUrl(helper.RedirectUri);
            await helper.Start(auth, null).ConfigureAwait(false);
            var code = helper.Code;
            var token = await client.GetToken(code, helper.RedirectUri).ConfigureAwait(false);

            client.CurrentAccessToken = token;
            await client.RefreshToken(token).ConfigureAwait(false);
            List<TaxTimeline> timeline = await client.Company.TaxTimeline().ConfigureAwait(false);
        }
    }
}
