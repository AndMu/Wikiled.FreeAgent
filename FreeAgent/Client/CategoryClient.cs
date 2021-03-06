using System.Threading.Tasks;
using RestSharp;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Client
{
    public class CategoryClient : BaseClient
    {
        // OMFG this is going to be so slow. Caching?
        private Categories all;

        public CategoryClient(FreeAgentClient client)
            : base(client)
        {
        }

        //need to add in the GET to have a parameter for the date filter

        public override string ResourceName => "categories";

        public async Task<Categories> All()
        {
            var request = CreateBasicRequest(Method.GET);
            var response = await Client.Execute<Categories>(request).ConfigureAwait(false);
            return response;
        }

        public async Task<Category> Single(string id)
        {
            if (all == null)
            {
                all = await All().ConfigureAwait(false);
            }

            foreach (var cat in all.admin_expenses_categories)
            {
                if (cat.nominal_code == id) return cat;
            }

            foreach (var cat in all.cost_of_sales_categories)
            {
                if (cat.nominal_code == id) return cat;
            }

            foreach (var cat in all.general_categories)
            {
                if (cat.nominal_code == id) return cat;
            }

            foreach (var cat in all.income_categories)
            {
                if (cat.nominal_code == id) return cat;
            }

            return null;
        }

        /* Disabled
         * 
         * The API comes back with a different root node based on the type.
         * FFS. This is insane. 
         * 
        public Categories Single(string id)
        {
            var request = CreateBasicRequest(Method.GET, "/{id}");
            request.AddParameter("id", id, ParameterType.UrlSegment);

            var response = Client.Execute<Categories>(request);

            if (response != null) return response;

            return null;    
        
        }
        */
    }
}
