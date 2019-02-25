using Wikiled.FreeAgent.Client;

namespace Wikiled.FreeAgent.Models
{
    //GET https://api.freeagent.com/v2/contacts

    //https://api.freeagent.com/v2/contacts

    public class Project : UpdatableModel, IRemoveUrlOnSerialization
    {
        public Project()
        {
            contact = "";
            name = "";
            status = ProjectStatus.Active;
            budget_units = ProjectBudgetUnits.Hours;
            currency = "GBP";
            billing_period = ProjectBillingPeriod.Hour;
        }

        public string billing_period { get; set; }

        public double budget { get; set; }

        public string budget_units { get; set; }

        public string contact { get; set; }

        public string contract_po_reference { get; set; }

        public string currency { get; set; }

        public string ends_on { get; set; }

        public double hours_per_day { get; set; }

        public bool is_ir35 { get; set; }

        public string name { get; set; }

        public double normal_billing_rate { get; set; }

        public string starts_on { get; set; }

        public string status { get; set; }

        public bool uses_project_invoice_sequence { get; set; }
    }
}
