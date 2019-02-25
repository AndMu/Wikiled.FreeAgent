using System;
using NUnit.Framework;
using Wikiled.FreeAgent.Client;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Tests
{
    [TestFixture]
    public class BankAccountFixture : ResourceFixture<BankAccountWrapper, BankAccountsWrapper, BankAccount>
    {
        public override ResourceClient<BankAccountWrapper, BankAccountsWrapper, BankAccount> ResourceClient => Client.BankAccount;

        public override bool CanDelete(BankAccount item)
        {
            return item.name.Contains("TEST");
        }

        public override void CheckSingleItem(BankAccount item)
        {
            Assert.IsNotEmpty(item.url);
            Assert.IsNotEmpty(item.type);
            Assert.IsNotEmpty(item.name);
            Assert.IsNotEmpty(item.bank_name);
        }

        public override void CompareSingleItem(BankAccount originalItem, BankAccount newItem)
        {
            Assert.IsNotNull(newItem);
            Assert.IsNotEmpty(newItem.url);
            Assert.AreEqual(newItem.account_number, originalItem.account_number);
            Assert.AreEqual(newItem.type, originalItem.type);
            Assert.AreEqual(newItem.opening_balance, originalItem.opening_balance);
        }

        public override BankAccount CreateSingleItemForInsert()
        {
            return new BankAccount
                   {
                       url = "",
                       opening_balance = 100,
                       type = BankAccountType.StandardBankAccount,
                       name = "Bank Account TEST " + DateTime.Now,
                       bank_name = "Test Bank"
                   };
        }
    }
}
