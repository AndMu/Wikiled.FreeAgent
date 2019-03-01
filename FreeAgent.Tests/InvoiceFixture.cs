using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Wikiled.FreeAgent.Client;
using Wikiled.FreeAgent.Extensions;
using Wikiled.FreeAgent.Models;
using Task = System.Threading.Tasks.Task;

namespace Wikiled.FreeAgent.Tests
{
    [TestFixture]
    public class InvoiceFixture : ResourceFixture<InvoiceWrapper, InvoicesWrapper, Invoice>
    {
        public override ResourceClient<InvoiceWrapper, InvoicesWrapper, Invoice> ResourceClient => Client.Invoice;

        public override async Task<bool> CanDelete(Invoice item)
        {
            if (item == null)
            {
                return false;
            }

            var newItem = await ResourceClient.Get(item.Id()).ConfigureAwait(false);

            if (newItem.invoice_items.Count == 0) return false;
            foreach (var invoiceItem in newItem.invoice_items)
            {
                if (invoiceItem.description.Contains("TEST")) return true;
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

        public override async Task<Invoice> CreateSingleItemForInsert()
        {
            var contact = await Client.Contact.All().FirstAsync();

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
        public async Task CanGetListForContact()
        {
            var contact = await Client.Contact.All().FirstAsync();

            var list = await Client.Invoice.AllForContact(contact.UrlId()).ToArray();

            CheckAllList(list);

            foreach (var item in list)
            {
                CheckSingleItem(item);
            }
        }

        [Test]
        public async Task CanGetListForProject()
        {
            var project = await Client.Project.All().FirstAsync();

            var list = await Client.Invoice.AllForProject(project.UrlId()).ToArray();
            CheckAllList(list);

            foreach (var item in list)
            {
                CheckSingleItem(item);
            }
        }

        [Test]
        public async Task CanGetListWithSingleCall()
        {
            var list = await Client.Invoice.AllWithFilter(InvoiceViewFilter.RecentOpenOrOverdue).ToArray();

            CheckAllList(list);

            foreach (var item in list)
            {
                CheckSingleItem(item);
            }
        }
    }
}
