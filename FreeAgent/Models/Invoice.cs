using System.Collections.Generic;
using Wikiled.FreeAgent.Client;

namespace Wikiled.FreeAgent.Models
{
    //GET https://api.freeagent.com/v2/contacts
    //https://api.freeagent.com/v2/contacts
    public class Invoice : UpdatableModel, IRemoveUrlOnSerialization, IRemoveRecurringOnSerialization
    {
        public Invoice()
        {
            invoice_items = new List<InvoiceItem>();
        }

        public string comments { get; set; }

        public string contact { get; set; }

        public string currency { get; set; }

        public string dated_on { get; set; }

        public double discount_percent { get; set; }

        public string due_on { get; set; }

        public double due_value { get; set; }

        public string ec_status { get; set; }

        public double exchange_rate { get; set; }

        public List<InvoiceItem> invoice_items { get; set; }

        public double net_value { get; set; }

        public double paid_value { get; set; }

        public int payment_terms_in_days { get; set; }

        public string project { get; set; }

        public string reference { get; set; }

        public double sales_tax_value { get; set; }

        public string status { get; set; }

        public string written_off_date { get; set; }
    }
}
