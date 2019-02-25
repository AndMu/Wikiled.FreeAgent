using System.Collections.Generic;
using NUnit.Framework;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Tests
{
    [TestFixture]
    public class CompanyFixture : BaseFixture
    {
        [Test]
        public void CanGetTaxTimeline()
        {
            List<TaxTimeline> timeline = Client.Company.TaxTimeline();

            Assert.IsNotNull(timeline);
            Assert.IsNotEmpty(timeline);
        }

        [Test]
        public void CanLoadCompany()
        {
            Company company = Client.Company.Single();

            Assert.IsNotNull(company);
            Assert.IsNotEmpty(company.name);
        }

        [SetUp]
        public void Setup()
        {
            SetupClient();
        }

        [Test]
        public void TaxTimelineHasContent()
        {
            List<TaxTimeline> timeline = Client.Company.TaxTimeline();

            Assert.IsNotNull(timeline);
            Assert.IsNotEmpty(timeline);

            foreach (TaxTimeline t in timeline)
            {
                Assert.IsNotEmpty(t.description);
            }
        }
    }
}
