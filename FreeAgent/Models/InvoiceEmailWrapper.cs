namespace Wikiled.FreeAgent.Models
{
    public class InvoiceEmailWrapper
    {
        public InvoiceEmailWrapper()
        {
            invoice = new InvoiceEmail();
        }

        public InvoiceEmail invoice { get; set; }
    }
}