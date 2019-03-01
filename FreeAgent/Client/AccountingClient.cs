using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<List<TrialBalanceSummary>> TrialBalanceSummary()
        {
            RestRequest request = CreateBasicRequest(Method.GET, "/trial_balance/summary");
            TrialBalanceSummaryWrapper response = await Client.Execute<TrialBalanceSummaryWrapper>(request).ConfigureAwait(false);
            return response?.trial_balance_summary;
        }
    }
}
