using Wikiled.FreeAgent.Client;

namespace Wikiled.FreeAgent.Models
{
    //GET https://api.freeagent.com/v2/bills

    //https://api.freeagent.com/v2/bills

    public class Bill : UpdatableModel, IRemoveUrlOnSerialization, IRemoveRecurringOnSerialization
    {
        public Bill()
        {
            ec_status = ExpenseECStatus.None;
        }

        public BillAttachment attachment { get; set; }

        public string category { get; set; }

        public string comments { get; set; }

        public string contact { get; set; }

        public string dated_on { get; set; }

        public string depreciation_schedule { get; set; }

        public string due_on { get; set; }

        public double due_value { get; set; }

        public int ec_status { get; set; }

        public double manual_sales_tax_amount { get; set; }

        public double paid_value { get; set; }

        public string project { get; set; }

        public double rebill_factor { get; set; }

        public string rebill_type { get; set; }

        public string recurring { get; set; }

        public string recurring_end_date { get; set; }

        public string reference { get; set; }

        public double sales_tax_rate { get; set; }

        public double sales_tax_value { get; set; }

        public double second_sales_tax_rate { get; set; }

        public string status { get; set; }

        public double total_value { get; set; }
    }
}
