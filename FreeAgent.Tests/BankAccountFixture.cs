using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Wikiled.FreeAgent.Client;
using Wikiled.FreeAgent.Models;
using Task = System.Threading.Tasks.Task;

namespace Wikiled.FreeAgent.Tests
{
    [TestFixture]
    public class BankAccountFixture : ResourceFixture<BankAccountWrapper, BankAccountsWrapper, BankAccount>
    {
        public override ResourceClient<BankAccountWrapper, BankAccountsWrapper, BankAccount> ResourceClient => Client.BankAccount;

        public override Task<bool> CanDelete(BankAccount item)
        {
            return Task.FromResult(item.name.Contains("TEST"));
        }

        public override void CheckSingleItem(BankAccount item)
        {
            Assert.IsNotEmpty(item.Url);
            Assert.IsNotEmpty(item.type);
            Assert.IsNotEmpty(item.name);
            Assert.IsNotEmpty(item.bank_name);
        }

        public override void CompareSingleItem(BankAccount originalItem, BankAccount newItem)
        {
            Assert.IsNotNull(newItem);
            Assert.IsNotEmpty(newItem.Url);
            Assert.AreEqual(newItem.account_number, originalItem.account_number);
            Assert.AreEqual(newItem.type, originalItem.type);
            Assert.AreEqual(newItem.opening_balance, originalItem.opening_balance);
        }

        public override Task<BankAccount> CreateSingleItemForInsert()
        {
            return Task.FromResult(new BankAccount
            {
                Url = "",
                opening_balance = 100,
                type = BankAccountType.StandardBankAccount,
                name = "Bank Account TEST " + DateTime.Now,
                bank_name = "Test Bank"
            });
        }
    }
}
