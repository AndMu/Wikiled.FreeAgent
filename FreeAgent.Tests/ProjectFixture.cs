using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Wikiled.FreeAgent.Client;
using Wikiled.FreeAgent.Extensions;
using Wikiled.FreeAgent.Models;
using Task = System.Threading.Tasks.Task;

namespace Wikiled.FreeAgent.Tests
{
    [TestFixture]
    public class ProjectFixture : ResourceFixture<ProjectWrapper, ProjectsWrapper, Project>
    {
        public override ResourceClient<ProjectWrapper, ProjectsWrapper, Project> ResourceClient => Client.Project;

        public override Task<bool> CanDelete(Project item)
        {
            return Task.FromResult(item.name.Contains("TEST"));
        }

        public override void CheckSingleItem(Project item)
        {
            Assert.IsNotEmpty(item.Url);
            Assert.IsNotEmpty(item.name);
            Assert.IsNotEmpty(item.contact);
            Assert.IsNotEmpty(item.status);
        }

        public override void CompareSingleItem(Project originalItem, Project newItem)
        {
            Assert.IsNotNull(newItem);
            Assert.IsNotEmpty(newItem.Url);
            Assert.AreEqual(newItem.name, originalItem.name);
            Assert.AreEqual(newItem.status, originalItem.status);
            Assert.AreEqual(newItem.budget_units, originalItem.budget_units);
            Assert.AreEqual(newItem.currency, originalItem.currency);
        }

        public override async Task<Project> CreateSingleItemForInsert()
        {
            var contact = await Client.Contact.All().FirstAsync();

            Assert.IsNotNull(contact);

            return new Project
                   {
                       Url = "",
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
