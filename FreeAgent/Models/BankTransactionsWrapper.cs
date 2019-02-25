using System.Collections.Generic;

namespace Wikiled.FreeAgent.Models
{
    public class BankTransactionsWrapper
    {
        public BankTransactionsWrapper()
        {
            bank_transactions = new List<BankTransaction>();
        }

        public List<BankTransaction> bank_transactions { get; set; }
    }
}