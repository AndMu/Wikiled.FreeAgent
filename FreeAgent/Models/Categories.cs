using System.Collections.Generic;

namespace Wikiled.FreeAgent.Models
{
    //https://dev.freeagent.com/docs/categories

    //https://api.freeagent.com/v2/categories

    public class Categories
    {
        public Categories()
        {
            admin_expenses_categories = new List<Category>();
            cost_of_sales_categories = new List<Category>();
            income_categories = new List<Category>();
            general_categories = new List<Category>();
        }

        public List<Category> admin_expenses_categories { get; set; }

        public List<Category> cost_of_sales_categories { get; set; }

        public List<Category> general_categories { get; set; }

        public List<Category> income_categories { get; set; }
    }
}
