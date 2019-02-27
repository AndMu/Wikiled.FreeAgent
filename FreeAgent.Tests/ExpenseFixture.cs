using System;
using System.Collections.Generic;
using NUnit.Framework;
using Wikiled.FreeAgent.Client;
using Wikiled.FreeAgent.Extensions;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Tests
{
    [TestFixture]
    public class ExpenseFixture : ResourceFixture<ExpenseWrapper, ExpensesWrapper, Expense>
    {
        public override ResourceClient<ExpenseWrapper, ExpensesWrapper, Expense> ResourceClient => Client.Expense;

        public override bool CanDelete(Expense item)
        {
            return false;
        }

        public override void CheckSingleItem(Expense item)
        {
            Assert.IsNotEmpty(item.url);
        }

        public override void CompareSingleItem(Expense originalItem, Expense newItem)
        {
            Assert.IsNotNull(newItem);
            Assert.IsNotEmpty(newItem.url);
            Assert.AreEqual(newItem.description, originalItem.description);

            //Assert.AreEqual(newItem.user, originalItem.user);
            Assert.AreEqual(newItem.dated_on, originalItem.dated_on);
        }

        public override Expense CreateSingleItemForInsert()
        {
            Assert.Ignore("IGNORING EXPENSE INSERTING UNTIL IT WORKS");
            var user = Client.User.Me;
            var cat = Client.Categories.Single("250");

            return new Expense
                   {
                       url = "",
                       user = user.UrlId(),
                       gross_value = 100.00,
                       description = "Expense TEST",
                       dated_on = DateTime.Now.ModelDate(),
                       category = cat.UrlId()

                       //recurring = false
                   };
        }

        public override void SetupClient()
        {
            base.SetupClient();
            GetAll = ExpenseAll;
        }

        public IEnumerable<Expense> ExpenseAll()
        {
            return Client.Expense.All(view: "recent");
        }
    }
}
