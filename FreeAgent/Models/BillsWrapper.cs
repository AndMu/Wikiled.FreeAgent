using System.Collections.Generic;

namespace Wikiled.FreeAgent.Models
{
    public class BillsWrapper
    {
        public BillsWrapper()
        {
            bills = new List<Bill>();
        }

        public List<Bill> bills { get; set; }
    }
}