using System.Collections.Generic;
using RestSharp;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Client
{
    public class AccountingClient : BaseClient
    {
        public AccountingClient(FreeAgentClient client)
            : base(client)
        {
        }

        public override string ResourceName => "accounting";

        public List<TrialBalanceSummary> TrialBalanceSummary()
        {
            var request = CreateBasicRequest(Method.GET, "/trial_balance/summary");
            var response = Client.Execute<TrialBalanceSummaryWrapper>(request);

            if (response != null) return response.trial_balance_summary;

            return null;
        }
    }
}
