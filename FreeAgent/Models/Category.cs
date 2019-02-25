namespace Wikiled.FreeAgent.Models
{
    public class Category : BaseModel
    {
        public bool allowable_for_tax { get; set; }

        public string auto_sales_tax_rate { get; set; }

        //public string url { get; set; }
        public string description { get; set; }

        public string nominal_code { get; set; }

        public string tax_reporting_name { get; set; }
    }
}