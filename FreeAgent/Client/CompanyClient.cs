using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Client
{
    public class CompanyClient : BaseClient
    {
        public CompanyClient(FreeAgentClient client)
            : base(client)
        {
        }

        public override string ResourceName => "company";

        public async Task<Company> Single()
        {
            var request = CreateBasicRequest(Method.GET);
            var response = await Client.Execute<CompanyWrapper>(request).ConfigureAwait(false);
            return response?.company;
        }

        public async Task<List<TaxTimeline>> TaxTimeline()
        {
            var request = CreateBasicRequest(Method.GET, "/tax_timeline");
            var response = await Client.Execute<TaxTimelineWrapper>(request).ConfigureAwait(false);
            return response?.timeline_items;
        }
    }
}
