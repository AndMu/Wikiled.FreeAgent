using System.Collections.Generic;

namespace Wikiled.FreeAgent.Models
{
    public class InvoicesWrapper
    {
        public InvoicesWrapper()
        {
            invoices = new List<Invoice>();
        }

        public List<Invoice> invoices { get; set; }
    }
}