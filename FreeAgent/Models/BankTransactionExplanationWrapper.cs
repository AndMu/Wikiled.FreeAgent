namespace Wikiled.FreeAgent.Models
{
    public class BankTransactionExplanationWrapper
    {
        public BankTransactionExplanationWrapper()
        {
            bank_transaction_explanation = null;
        }

        public BankTransactionExplanation bank_transaction_explanation { get; set; }
    }
}