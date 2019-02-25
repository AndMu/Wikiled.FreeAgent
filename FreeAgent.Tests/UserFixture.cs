using NUnit.Framework;
using Wikiled.FreeAgent.Client;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Tests
{
    [TestFixture]
    public class UserFixture : ResourceFixture<UserWrapper, UsersWrapper, User>
    {
        public override ResourceClient<UserWrapper, UsersWrapper, User> ResourceClient => Client.User;

        public override bool CanDelete(User item)
        {
            return item.first_name.Contains("TEST");
        }

        public override void CheckSingleItem(User item)
        {
            Assert.IsNotEmpty(item.url);
            Assert.IsNotEmpty(item.first_name);
            Assert.IsNotEmpty(item.last_name);
            Assert.IsNotEmpty(item.email);
            Assert.IsNotEmpty(item.role);
        }

        public override void CompareSingleItem(User originalItem, User newItem)
        {
            Assert.IsNotNull(newItem);
            Assert.IsNotEmpty(newItem.url);
            Assert.AreEqual(newItem.first_name, originalItem.first_name);
            Assert.AreEqual(newItem.last_name, originalItem.last_name);
            Assert.AreEqual(newItem.email, originalItem.email);
        }

        public override User CreateSingleItemForInsert()
        {
            return new User
                   {
                       url = "",
                       first_name = "Nic TEST",
                       last_name = "Wise",
                       email = "nic.wise@mycompany.com",
                       password = "foobarbaz",
                       password_confirmation = "foobarbaz",
                       opening_mileage = 100,
                       permission_level = (int)UserPermission.Full,
                       role = UserRole.Director
                   };
        }

        [Test]
        public void CanLoadMe()
        {
            var me = Client.User.Me;

            Assert.IsNotNull(me);
            Assert.IsNotEmpty(me.first_name);
            Assert.IsNotEmpty(me.last_name);
            Assert.IsNotEmpty(me.email);
        }
    }
}
