using System.Collections.Generic;

namespace Wikiled.FreeAgent.Models
{
    public class BankAccountsWrapper
    {
        public BankAccountsWrapper()
        {
            bank_accounts = new List<BankAccount>();
        }

        public List<BankAccount> bank_accounts { get; set; }
    }
}