using System;
using System.Linq;
using NUnit.Framework;
using Wikiled.FreeAgent.Client;
using Wikiled.FreeAgent.Extensions;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Tests
{
    public class TimeslipFixture : ResourceFixture<TimeslipWrapper, TimeslipsWrapper, Timeslip>
    {
        public override ResourceClient<TimeslipWrapper, TimeslipsWrapper, Timeslip> ResourceClient => Client.Timeslip;

        public override bool CanDelete(Timeslip item)
        {
            //return false;
            if (string.IsNullOrEmpty(item.comment)) return false;
            return item.comment.Contains("TEST");
        }

        public override void CheckSingleItem(Timeslip item)
        {
            Assert.IsNotEmpty(item.url);
            Assert.IsNotEmpty(item.dated_on);
            Assert.IsNotEmpty(item.project);
            Assert.IsNotEmpty(item.task);
            Assert.IsNotEmpty(item.user);
        }

        public override void CompareSingleItem(Timeslip originalItem, Timeslip newItem)
        {
            Assert.IsNotNull(newItem);
            Assert.IsNotEmpty(newItem.url);
            Assert.IsTrue(newItem.user.EndsWith(originalItem.user));
            Assert.IsTrue(newItem.project.EndsWith(originalItem.project));
            Assert.IsTrue(newItem.task.EndsWith(originalItem.task));
            Assert.AreEqual(newItem.dated_on, originalItem.dated_on);
            Assert.AreEqual(newItem.hours, originalItem.hours);
        }

        public override void Configure()
        {
            ExecuteCanGetList = false;
            ExecuteCanGetListWithContent = false;

            //ExecuteCanDeleteAndCleanup = false;
        }

        public override Timeslip CreateSingleItemForInsert()
        {
            var user = Client.User.Me;
            var project = Client.Project.All().First();
            var task = Client.Task.AllByProject(project.Id()).First();

            return new Timeslip
                   {
                       url = "",
                       user = user.UrlId(),
                       project = project.UrlId(),
                       task = task.UrlId(),
                       dated_on = DateTime.Now.ModelDate(),
                       hours = 6.5,
                       comment = "This is a TEST"
                   };
        }

        [Test]
        public void CanGetListOfTimeslips()
        {
            var list = Client.Timeslip.All(DateTime.Now.AddMonths(-6).ModelDateTime(), DateTime.Now.ModelDateTime());

            Assert.IsNotNull(list);
        }

        [Test]
        public void CanGetListOfTimeslipWithContent()
        {
            var list = Client.Timeslip.All(DateTime.Now.AddMonths(-6).ModelDateTime(), DateTime.Now.ModelDateTime());

            CheckAllList(list);

            foreach (var item in list)
            {
                CheckSingleItem(item);
            }
        }
    }
}
