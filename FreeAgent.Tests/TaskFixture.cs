using System;
using System.Reactive.Linq;
using NUnit.Framework;
using Wikiled.FreeAgent.Extensions;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Tests
{
    [TestFixture]
    public class TaskFixture : BaseFixture
    {
        [Test]
        public async System.Threading.Tasks.Task CanCreateSingleTaskForProject()
        {
            var project = await Client.Project.All().FirstAsync();

            var task = new Task
                       {
                           name = "Task TEST " + DateTime.Now,
                           is_billable = true,
                           billing_rate = 400,
                           billing_period = TaskBillingPeriod.Day,
                           status = TaskStatus.Active,
                           project = ""

                           //project = project.UrlId()
                       };

            var newTask = await Client.Task.Put(task, project.UrlId()).ConfigureAwait(false);

            CompareSingleItem(task, newTask);
        }

        [Test]
        public async System.Threading.Tasks.Task CanDeleteAndCleanup()
        {
            var project = await Client.Project.All().FirstAsync();

            var tasks = await Client.Task.AllByProject(project.Id()).ToArray();

            foreach (var item in tasks)
            {
                if (!item.name.Contains("TEST")) continue;

                Client.Task.Delete(item.Id());

                var deleted = Client.Task.Get(item.Id());

                Assert.IsNull(deleted);
            }
        }

        [Test]
        public async System.Threading.Tasks.Task CanGetSingleTaskForProject()
        {
            var project = await Client.Project.All().FirstAsync();

            var tasks = await Client.Task.AllByProject(project.Id()).ToArray();

            Assert.IsNotEmpty(tasks);

            foreach (var task in tasks)
            {
                var newTask = await Client.Task.Get(task.Id()).ConfigureAwait(false);
                Assert.IsNotNull(newTask);
                Assert.IsNotEmpty(newTask.name);
            }
        }

        [Test]
        public async System.Threading.Tasks.Task CanGetTasksForProject()
        {
            var project = await Client.Project.All().FirstAsync();

            var tasks = await Client.Task.AllByProject(project.Id()).ToArray();

            Assert.IsNotEmpty(tasks);
        }

        public void CompareSingleItem(Task originalItem, Task newItem)
        {
            Assert.IsNotNull(newItem);
            Assert.IsNotEmpty(newItem.Url);

            Assert.AreEqual(originalItem.name, newItem.name);
            Assert.AreEqual(originalItem.billing_period, newItem.billing_period);
            Assert.AreEqual(originalItem.billing_rate, newItem.billing_rate);
            Assert.AreEqual(originalItem.status, newItem.status);
        }

        [SetUp]
        public async System.Threading.Tasks.Task Setup()
        {
            await SetupClient().ConfigureAwait(false);
        }
    }
}
