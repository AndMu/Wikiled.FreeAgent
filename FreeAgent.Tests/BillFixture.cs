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
    public class BillFixture : ResourceFixture<BillWrapper, BillsWrapper, Bill>
    {
        public override ResourceClient<BillWrapper, BillsWrapper, Bill> ResourceClient => Client.Bill;

        public override Task<bool> CanDelete(Bill item)
        {
            return Task.FromResult(false);
        }

        public override void CheckSingleItem(Bill item)
        {
            Assert.IsNotEmpty(item.Url);
        }

        public override void CompareSingleItem(Bill originalItem, Bill newItem)
        {
            Assert.IsNotNull(newItem);
            Assert.IsNotEmpty(newItem.Url);
            Assert.AreEqual(newItem.status, originalItem.status);

            //Assert.AreEqual(newItem.user, originalItem.user);
            Assert.AreEqual(newItem.dated_on, originalItem.dated_on);
        }

        public override async Task<Bill> CreateSingleItemForInsert()
        {
            var user = await Client.User.ResolveMe().ConfigureAwait(false);
            var cat = await Client.Categories.Single("250").ConfigureAwait(false);

            return new Bill
                   {
                       Url = "",
                       dated_on = DateTime.Now.ModelDate(),
                       category = cat.UrlId()

                       //recurring = false
                   };
        }

        public override async Task SetupClient()
        {
            await base.SetupClient().ConfigureAwait(false);
            GetAll = BillAll;
        }

        public IObservable<Bill> BillAll()
        {
            return Client.Bill.All("recent");
        }
    }
}
