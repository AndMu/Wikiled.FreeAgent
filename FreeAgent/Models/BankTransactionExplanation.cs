namespace Wikiled.FreeAgent.Models
{
    public class BankTransactionExplanation : UpdatableModel
    {
        public ExpenseAttachment attachment { get; set; }

        public string bank_account { get; set; }

        public string bank_transaction { get; set; }

        public string category { get; set; }

        public string dated_on { get; set; }

        public string description { get; set; }

        public double foreign_currency_value { get; set; }

        public double gross_value { get; set; }

        public double manual_sales_tax_amount { get; set; }

        public double rebill_factor { get; set; }

        public string rebill_type { get; set; }

        public double sales_tax_rate { get; set; }
    }
}