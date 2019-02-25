namespace Wikiled.FreeAgent.Models
{
    public class BankTransactionWrapper
    {
        public BankTransactionWrapper()
        {
            bank_transaction = null;
        }

        public BankTransaction bank_transaction { get; set; }
    }
}