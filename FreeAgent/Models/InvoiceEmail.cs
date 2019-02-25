namespace Wikiled.FreeAgent.Models
{
    public class InvoiceEmail
    {
        public InvoiceEmail()
        {
            email = new InvoiceEmailDetail();
        }

        public InvoiceEmailDetail email { get; set; }
    }
}