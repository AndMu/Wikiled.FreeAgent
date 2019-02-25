using NUnit.Framework;

namespace Wikiled.FreeAgent.Tests
{
    [TestFixture]
    public class CategoryFixture : BaseFixture
    {
        [Test]
        public void CanGetListOfCategories()
        {
            var cats = Client.Categories.All();
            Assert.IsNotEmpty(cats.admin_expenses_categories);
            Assert.IsNotEmpty(cats.cost_of_sales_categories);
            Assert.IsNotEmpty(cats.general_categories);
            Assert.IsNotEmpty(cats.income_categories);
        }

        [SetUp]
        public void Setup()
        {
            SetupClient();
        }

        /*
         * Disabled as I dont need it, and the API design is.... interesting.
        [Test]
        public void CanGetSingleCategory()
        {
            var cats = Client.Categories.Single("249");

            Assert.IsNotEmpty(cats.income_categories);
        }
        */
    }
}
