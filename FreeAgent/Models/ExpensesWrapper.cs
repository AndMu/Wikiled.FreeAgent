using System.Collections.Generic;

namespace Wikiled.FreeAgent.Models
{
    public class ExpensesWrapper
    {
        public ExpensesWrapper()
        {
            expenses = new List<Expense>();
        }

        public List<Expense> expenses { get; set; }
    }
}