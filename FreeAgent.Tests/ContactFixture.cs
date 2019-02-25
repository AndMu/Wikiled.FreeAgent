using System;
using NUnit.Framework;
using Wikiled.FreeAgent.Client;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Tests
{
    [TestFixture]
    public class ContactFixture : ResourceFixture<ContactWrapper, ContactsWrapper, Contact>
    {
        public override ResourceClient<ContactWrapper, ContactsWrapper, Contact> ResourceClient => Client.Contact;

        public override bool CanDelete(Contact item)
        {
            return item.first_name.Contains("TEST");
        }

        public override void CheckSingleItem(Contact item)
        {
            Assert.IsNotEmpty(item.url);

            //Assert.IsNotEmpty(contact.organisation_name);
            //Assert.IsNotEmpty(contact.first_name);
            //Assert.IsNotEmpty(contact.last_name);
        }

        public override void CompareSingleItem(Contact originalItem, Contact newItem)
        {
            Assert.IsNotNull(newItem);
            Assert.IsNotEmpty(newItem.url);
            Assert.AreEqual(newItem.first_name, originalItem.first_name);
            Assert.AreEqual(newItem.last_name, originalItem.last_name);
            Assert.AreEqual(newItem.address1, originalItem.address1);
        }

        public override Contact CreateSingleItemForInsert()
        {
            return new Contact
                   {
                       url = "",
                       first_name = "Nic TEST",
                       last_name = "Wise",
                       organisation_name = "foo",
                       address1 = DateTime.Now.ToLongTimeString()
                   };
        }
    }
}
