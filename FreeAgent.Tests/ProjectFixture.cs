using System.Linq;
using NUnit.Framework;
using Wikiled.FreeAgent.Client;
using Wikiled.FreeAgent.Extensions;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Tests
{
    [TestFixture]
    public class ProjectFixture : ResourceFixture<ProjectWrapper, ProjectsWrapper, Project>
    {
        public override ResourceClient<ProjectWrapper, ProjectsWrapper, Project> ResourceClient => Client.Project;

        public override bool CanDelete(Project item)
        {
            return item.name.Contains("TEST");
        }

        public override void CheckSingleItem(Project item)
        {
            Assert.IsNotEmpty(item.url);
            Assert.IsNotEmpty(item.name);
            Assert.IsNotEmpty(item.contact);
            Assert.IsNotEmpty(item.status);
        }

        public override void CompareSingleItem(Project originalItem, Project newItem)
        {
            Assert.IsNotNull(newItem);
            Assert.IsNotEmpty(newItem.url);
            Assert.AreEqual(newItem.name, originalItem.name);
            Assert.AreEqual(newItem.status, originalItem.status);
            Assert.AreEqual(newItem.budget_units, originalItem.budget_units);
            Assert.AreEqual(newItem.currency, originalItem.currency);
        }

        public override Project CreateSingleItemForInsert()
        {
            var contact = Client.Contact.All().First();

            Assert.IsNotNull(contact);

            return new Project
                   {
                       url = "",
                       contact = contact.UrlId(),
                       name = "project TEST",
                       status = ProjectStatus.Active,
                       budget_units = ProjectBudgetUnits.Days,
                       hours_per_day = 7.5,
                       billing_period = ProjectBillingPeriod.Day,
                       normal_billing_rate = 450,
                       currency = "GBP"
                   };
        }
    }
}
