using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Collections.Generic;
using Wikiled.Common.Logging;
using Wikiled.FreeAgent.Client;
using Wikiled.FreeAgent.Models;
using Wikiled.FreeAgent.TestApp.Auth;

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
            string code;
            AccessTokenData token;
            var helper = new OAuthHelper(new Logger<OAuthHelper>(ApplicationLogging.LoggerFactory));
            var auth = client.BuildAuthorizeUrl(helper.RedirectUri);
            await helper.Start(auth, null).ConfigureAwait(false);
            code = helper.Code;
            helper.Start(auth, null);
            token = client.GetAccessToken(code, helper.RedirectUri);
            //await helper.Start(null).ConfigureAwait(false);
            //token.access_token = helper.Code;

            client.CurrentAccessToken = token;
            client.RefreshAccessToken();
            List<TaxTimeline> timeline = client.Company.TaxTimeline();
        }
    }
}
