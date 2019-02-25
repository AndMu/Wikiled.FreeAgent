using System.Collections.Generic;

namespace Wikiled.FreeAgent.Models
{
    public class BankTransactionExplanationsWrapper
    {
        public BankTransactionExplanationsWrapper()
        {
            bank_transaction_explanations = new List<BankTransactionExplanation>();
        }

        public List<BankTransactionExplanation> bank_transaction_explanations { get; set; }
    }
}