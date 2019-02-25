using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wikiled.FreeAgent.Client;
using Wikiled.FreeAgent.Extensions;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Tests
{
    [TestFixture]
    public class InvoiceFixture : ResourceFixture<InvoiceWrapper, InvoicesWrapper, Invoice>
    {
        public override ResourceClient<InvoiceWrapper, InvoicesWrapper, Invoice> ResourceClient => Client.Invoice;

        public override bool CanDelete(Invoice item)
        {
            if (item == null) return false;

            var newitem = ResourceClient.Get(item.Id());

            if (newitem.invoice_items.Count == 0) return false;
            foreach (var invoiceitem in newitem.invoice_items)
            {
                if (invoiceitem.description.Contains("TEST")) return true;
            }

            return false;
        }

        public override void CheckSingleItem(Invoice item)
        {
            Assert.IsNotEmpty(item.url);
            Assert.IsNotEmpty(item.invoice_items);
        }

        public override void CompareSingleItem(Invoice originalItem, Invoice newItem)
        {
            Assert.IsNotNull(newItem);
            Assert.IsNotEmpty(newItem.url);
        }

        public override void Configure()
        {
            base.Configure();
            ExecuteCanGetListWithContent = false;
        }

        public override Invoice CreateSingleItemForInsert()
        {
            var contact = Client.Contact.All().First();

            Assert.IsNotNull(contact);

            //find a project for this contact

            var items = new List<InvoiceItem>();
            items.Add(
                new InvoiceItem
                {
                    item_type = InvoiceItemType.Products,
                    quantity = 1,
                    price = 100,
                    description = "some item TEST"
                });

            return new Invoice
                   {
                       url = "",
                       contact = contact.UrlId(),
                       status = InvoiceStatus.Draft,
                       dated_on = DateTime.Now.ModelDateTime(),
                       payment_terms_in_days = 25,
                       invoice_items = items
                   };
        }

        [Test]
        public void CanGetListForContact()
        {
            var contact = Client.Contact.All().First();

            var list = Client.Invoice.AllForContact(contact.UrlId());

            CheckAllList(list);

            foreach (var item in list)
            {
                CheckSingleItem(item);
            }
        }

        [Test]
        public void CanGetListForProject()
        {
            var project = Client.Project.All().First();

            var list = Client.Invoice.AllForProject(project.UrlId());

            CheckAllList(list);

            foreach (var item in list)
            {
                CheckSingleItem(item);
            }
        }

        [Test]
        public void CanGetListWithSingleCall()
        {
            var list = Client.Invoice.AllWithFilter(InvoiceViewFilter.RecentOpenOrOverdue);

            CheckAllList(list);

            foreach (var item in list)
            {
                CheckSingleItem(item);
            }
        }

        //should add invoice timeline in here? 
    }
}
