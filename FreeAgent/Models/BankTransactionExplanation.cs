using Newtonsoft.Json;

namespace Wikiled.FreeAgent.Models
{
    public class BankTransactionExplanation : UpdatableModel
    {
        [JsonProperty("attachment")]
        public ExpenseAttachment Attachment { get; set; }

        [JsonProperty("bank_account")]
        public string BankAccount { get; set; }

        [JsonProperty("bank_transaction")]
        public string BankTransaction { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("dated_on")]
        public string DatedOn { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("foreign_currency_value")]
        public double ForeignCurrencyValue { get; set; }

        [JsonProperty("gross_value")]
        public double GrossValue { get; set; }

        [JsonProperty("manual_sales_tax_amount")]
        public double ManualSalesTaxAmount { get; set; }

        [JsonProperty("rebill_factor")]
        public double RebillFactor { get; set; }

        [JsonProperty("rebill_type")]
        public string RebillType { get; set; }

        [JsonProperty("sales_tax_rate")]
        public double SalesTaxRate { get; set; }
    }
}