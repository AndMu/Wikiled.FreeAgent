using Wikiled.FreeAgent.Client;

namespace Wikiled.FreeAgent.Models
{
    //GET https://api.freeagent.com/v2/contacts

    //https://api.freeagent.com/v2/contacts

    public class Expense : UpdatableModel, IRemoveUrlOnSerialization, IRemoveRecurringOnSerialization
    {
        public Expense()
        {
            //recurring = false;
            have_vat_receipt = false;
            ec_status = ExpenseECStatus.None;
        }

        public ExpenseAttachment attachment { get; set; }

        public string category { get; set; }

        public string currency { get; set; }

        public string dated_on { get; set; }

        public string description { get; set; }

        public int ec_status { get; set; }

        public int engine_size_index { get; set; }

        public int engine_type_index { get; set; }

        public double gross_value { get; set; }

        public bool have_vat_receipt { get; set; }

        public double initial_rate_mileage { get; set; }

        public double manual_sales_tax_amount { get; set; }

        public double mileage { get; set; }

        public string project { get; set; }

        public double rebill_factor { get; set; }

        public double rebill_mileage_rate { get; set; }

        //public bool recurring { get; set;}
        //public string recurring_end_date { get; set; }
        public string rebill_type { get; set; }

        public string receipt_reference { get; set; }

        public double reclaim_mileage_rate { get; set; }

        public double sales_tax_rate { get; set; }

        public string user { get; set; }

        public string vehicle_type { get; set; }
    }
}
