using System.Threading.Tasks;
using NUnit.Framework;

namespace Wikiled.FreeAgent.Tests
{
    [TestFixture]
    public class CategoryFixture : BaseFixture
    {
        [Test]
        public async Task CanGetListOfCategories()
        {
            var cats = await Client.Categories.All();
            Assert.IsNotEmpty(cats.admin_expenses_categories);
            Assert.IsNotEmpty(cats.cost_of_sales_categories);
            Assert.IsNotEmpty(cats.general_categories);
            Assert.IsNotEmpty(cats.income_categories);
        }

        [SetUp]
        public async Task Setup()
        {
            await SetupClient().ConfigureAwait(false);
        }
    }
}
