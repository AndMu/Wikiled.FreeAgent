namespace Wikiled.FreeAgent.Models
{
    public class InvoiceItem
    {
        public string category { get; set; }

        public string description { get; set; }

        public string item_type { get; set; }

        public int position { get; set; }

        public double price { get; set; }

        public double quantity { get; set; }

        public double sales_tax_rate { get; set; }

        public double second_sales_tax_rate { get; set; }

        public string url { get; set; }
    }
}