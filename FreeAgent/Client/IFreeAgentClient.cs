using Wikiled.FreeAgent.Auth;
using Wikiled.FreeAgent.Helpers;

namespace Wikiled.FreeAgent.Client
{
    public interface IFreeAgentClient
    {
        AccountingClient Accounting { get; set; }

        BankAccountClient BankAccount { get; set; }

        BankTransactionClient BankTransaction { get; set; }

        BankTransactionExplanationClient BankTransactionExplanation { get; set; }

        BillClient Bill { get; set; }

        CategoryClient Categories { get; set; }

        CompanyClient Company { get; set; }

        ContactClient Contact { get; set; }

        ExpenseClient Expense { get; set; }

        InvoiceClient Invoice { get; set; }

        ProjectClient Project { get; set; }

        TaskClient Task { get; set; }

        TimeslipClient Timeslip { get; set; }

        UserClient User { get; }

        AccessTokenData CurrentAccessToken { get; set; }

        RequestHelper Helper { get; }
        
        void SetProxy();
    }
}