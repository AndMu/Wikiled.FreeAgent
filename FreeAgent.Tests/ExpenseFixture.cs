using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Wikiled.FreeAgent.Client;
using Wikiled.FreeAgent.Extensions;
using Wikiled.FreeAgent.Models;
using Task = System.Threading.Tasks.Task;

namespace Wikiled.FreeAgent.Tests
{
    [TestFixture]
    public class ExpenseFixture : ResourceFixture<ExpenseWrapper, ExpensesWrapper, Expense>
    {
        public override ResourceClient<ExpenseWrapper, ExpensesWrapper, Expense> ResourceClient => Client.Expense;

        public override Task<bool> CanDelete(Expense item)
        {
            return Task.FromResult(false);
        }

        public override void CheckSingleItem(Expense item)
        {
            Assert.IsNotEmpty(item.Url);
        }

        public override void CompareSingleItem(Expense originalItem, Expense newItem)
        {
            Assert.IsNotNull(newItem);
            Assert.IsNotEmpty(newItem.Url);
            Assert.AreEqual(newItem.description, originalItem.description);

            //Assert.AreEqual(newItem.user, originalItem.user);
            Assert.AreEqual(newItem.dated_on, originalItem.dated_on);
        }

        public override async Task<Expense> CreateSingleItemForInsert()
        {
            var user = await Client.User.ResolveMe().ConfigureAwait(false);
            var cat = await Client.Categories.Single("250").ConfigureAwait(false);

            return new Expense
                   {
                       Url = "",
                       user = user.UrlId(),
                       gross_value = 100.00,
                       description = "Expense TEST",
                       dated_on = DateTime.Now.ModelDate(),
                       category = cat.UrlId()

                       //recurring = false
                   };
        }

        public override async Task SetupClient()
        {
            await base.SetupClient().ConfigureAwait(false);
            GetAll = ExpenseAll;
        }

        public IObservable<Expense> ExpenseAll()
        {
            return Client.Expense.All(view: "recent");
        }
    }
}
