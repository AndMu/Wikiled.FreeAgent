using System.Collections.Generic;
using RestSharp;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Client
{
    public class BankTransactionClient : ResourceClient<BankTransactionWrapper, BankTransactionsWrapper, BankTransaction>
    {
        public BankTransactionClient(FreeAgentClient client)
            : base(client)
        {
        }

        //need to add in the GET to have a parameter for the date filter

        public override string ResourceName => "bank_transactions";

        public override List<BankTransaction> ListFromWrapper(BankTransactionsWrapper wrapper)
        {
            return wrapper.bank_transactions;
        }

        public override BankTransaction SingleFromWrapper(BankTransactionWrapper wrapper)
        {
            return wrapper.bank_transaction;
        }

        public override BankTransactionWrapper WrapperFromSingle(BankTransaction single)
        {
            return new BankTransactionWrapper {bank_transaction = single};
        }

        public List<BankTransaction> AllForAccount(string bankAccountId, string fromDate, string toDate)
        {
            return All(
                r =>
                {
                    r.AddParameter("bank_account", bankAccountId, ParameterType.GetOrPost);
                    r.AddParameter("from_date", fromDate, ParameterType.GetOrPost);
                    r.AddParameter("to_date", toDate, ParameterType.GetOrPost);
                });
        }
    }
}