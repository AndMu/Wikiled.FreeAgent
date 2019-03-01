using System.Collections.Generic;
using NUnit.Framework;
using Wikiled.FreeAgent.Models;
using Task = System.Threading.Tasks.Task;

namespace Wikiled.FreeAgent.Tests
{
    [TestFixture]
    public class CompanyFixture : BaseFixture
    {
        [Test]
        public async Task CanGetTaxTimeline()
        {
            List<TaxTimeline> timeline = await Client.Company.TaxTimeline().ConfigureAwait(false);

            Assert.IsNotNull(timeline);
            Assert.IsNotEmpty(timeline);
        }

        [Test]
        public async Task CanLoadCompany()
        {
            Company company = await Client.Company.Single().ConfigureAwait(false);
            Assert.IsNotNull(company);
            Assert.IsNotEmpty(company.name);
        }

        [SetUp]
        public async Task Setup()
        {
            await SetupClient().ConfigureAwait(false);
        }

        [Test]
        public async Task TaxTimelineHasContent()
        {
            List<TaxTimeline> timeline = await Client.Company.TaxTimeline().ConfigureAwait(false);

            Assert.IsNotNull(timeline);
            Assert.IsNotEmpty(timeline);

            foreach (TaxTimeline t in timeline)
            {
                Assert.IsNotEmpty(t.description);
            }
        }
    }
}
