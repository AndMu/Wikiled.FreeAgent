using System;
using System.Collections.Generic;
using NUnit.Framework;
using Wikiled.FreeAgent.Client;
using Wikiled.FreeAgent.Extensions;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Tests
{
    [TestFixture]
    public class BillFixture : ResourceFixture<BillWrapper, BillsWrapper, Bill>
    {
        public override ResourceClient<BillWrapper, BillsWrapper, Bill> ResourceClient => Client.Bill;

        public override bool CanDelete(Bill item)
        {
            return false;
            return item.status.Contains("TEST");
        }

        public override void CheckSingleItem(Bill item)
        {
            Assert.IsNotEmpty(item.url);

            //Assert.IsNotEmpty(contact.organisation_name);
            //Assert.IsNotEmpty(contact.first_name);
            //Assert.IsNotEmpty(contact.last_name);
        }

        public override void CompareSingleItem(Bill originalItem, Bill newItem)
        {
            Assert.IsNotNull(newItem);
            Assert.IsNotEmpty(newItem.url);
            Assert.AreEqual(newItem.status, originalItem.status);

            //Assert.AreEqual(newItem.user, originalItem.user);
            Assert.AreEqual(newItem.dated_on, originalItem.dated_on);
        }

        public override Bill CreateSingleItemForInsert()
        {
            Assert.Ignore("IGNORING Bill INSERTING UNTIL IT WORKS");
            var user = Client.User.Me;
            var cat = Client.Categories.Single("250");

            return new Bill
                   {
                       url = "",
                       dated_on = DateTime.Now.ModelDate(),
                       category = cat.UrlId()

                       //recurring = false
                   };
        }

        public override void SetupClient()
        {
            base.SetupClient();
            GetAll = BillAll;
        }

        public IEnumerable<Bill> BillAll()
        {
            return Client.Bill.All("recent");
        }
    }
}
