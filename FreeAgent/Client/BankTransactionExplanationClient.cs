using System;
using System.Collections.Generic;
using RestSharp;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Client
{
    public class BankTransactionExplanationClient : ResourceClient<BankTransactionExplanationWrapper, BankTransactionExplanationsWrapper, BankTransactionExplanation>
    {
        public BankTransactionExplanationClient(FreeAgentClient client)
            : base(client)
        {
        }

        public override string ResourceName => "bank_transaction_explanations";

        public IObservable<BankTransactionExplanation> All(string account)
        {
            return All(r => { r.AddParameter("bank_transaction", account, ParameterType.GetOrPost); });
        }

        public override List<BankTransactionExplanation> ListFromWrapper(BankTransactionExplanationsWrapper wrapper)
        {
            return wrapper.bank_transaction_explanations;
        }

        public override BankTransactionExplanation SingleFromWrapper(BankTransactionExplanationWrapper wrapper)
        {
            return wrapper.bank_transaction_explanation;
        }

        public override BankTransactionExplanationWrapper WrapperFromSingle(BankTransactionExplanation single)
        {
            return new BankTransactionExplanationWrapper {bank_transaction_explanation = single};
        }
    }
}