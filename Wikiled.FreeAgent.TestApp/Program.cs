using System;
using System.Collections.Generic;
using System.Diagnostics;
using Wikiled.FreeAgent.Client;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            FreeAgentClient.UseSandbox = true;
            var auth = client.BuildAuthorizeUrl();
            //Process.Start("cmd " + auth);
           
            string code = client.ExtractCodeFromUrl("https://developers.google.com/oauthplayground/?code=1GnXmdmCuGE092uyYiHsrz_1Wxit5u-GPcQA2oiQy&state=foo");
            var newToken = client.GetAccessToken(code, null);
            //client.BankTransactionExplanation.All();
            List<TaxTimeline> timeline = client.Company.TaxTimeline();
        }
    }
}
