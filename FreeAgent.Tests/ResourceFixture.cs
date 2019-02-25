using System;
using System.Collections.Generic;
using NUnit.Framework;
using Wikiled.FreeAgent.Client;
using Wikiled.FreeAgent.Extensions;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Tests
{
    public class ResourceFixture<TSingleWrapper, TListWrapper, TSingle> : BaseFixture
        where TSingle : BaseModel
        where TListWrapper : new()
        where TSingleWrapper : new()
    {
        protected bool ExecuteCanGetList = true, ExecuteCanGetListWithContent = true, ExecuteCanLoadById = true, ExecuteCanCreateSingle = true, ExecuteCanDeleteAndCleanup = true;

        protected Func<IEnumerable<TSingle>> GetAll;

        public virtual ResourceClient<TSingleWrapper, TListWrapper, TSingle> ResourceClient => throw new NotImplementedException("oops!");

        public virtual bool CanDelete(TSingle item)
        {
            return false;
        }

        public virtual void CheckSingleItem(TSingle item)
        {
        }

        public virtual void CompareSingleItem(TSingle originalItem, TSingle newItem)
        {
            throw new NotImplementedException("needs to be overridden");
        }

        public virtual TSingle CreateSingleItemForInsert()
        {
            throw new NotImplementedException("needs to be overridden");
        }

        public override void SetupClient()
        {
            base.SetupClient();
            GetAll = ResourceFixtureAll;
        }

        [Test]
        public void CanCreateSingle()
        {
            if (!ExecuteCanCreateSingle)
            {
                Assert.Ignore("ExecuteCanCreateSingle is being ignored");
                return;
            }

            TSingle item = CreateSingleItemForInsert();

            TSingle result = ResourceClient.Put(item);

            CompareSingleItem(item, result);
        }

        [Test]
        public void CanDeleteAndCleanup()
        {
            if (!ExecuteCanDeleteAndCleanup)
            {
                Assert.Ignore("ExecuteCanDeleteAndCleanup is being ignored");
                return;
            }

            var items = GetAll();

            CheckAllList(items);
            foreach (var item in items)
            {
                if (!CanDelete(item)) continue;

                ResourceClient.Delete(item.Id());

                var deletedclient = ResourceClient.Get(item.Id());

                Assert.IsNull(deletedclient);
            }
        }

        [Test]
        public void CanGetList()
        {
            if (!ExecuteCanGetList)
            {
                Assert.Ignore("CanGetList is being ignored");
                return;
            }

            var list = GetAll();

            Assert.IsNotNull(list);
        }

        [Test]
        public void CanGetListWithContent()
        {
            if (!ExecuteCanGetListWithContent)
            {
                Assert.Ignore("ExecuteCanGetListWithContent is being ignored");
                return;
            }

            var list = GetAll();

            CheckAllList(list);

            foreach (var item in list)
            {
                CheckSingleItem(item);
            }
        }

        [Test]
        public void CanLoadById()
        {
            if (!ExecuteCanLoadById)
            {
                Assert.Ignore("ExecuteCanLoadById is being ignored");
                return;
            }

            var items = GetAll();
            CheckAllList(items);

            foreach (var item in items)
            {
                var newitem = ResourceClient.Get(item.Id());

                CompareSingleItem(item, newitem);
            }
        }

        public void CheckAllList(IEnumerable<TSingle> list)
        {
            Assert.IsNotNull(list);
            Assert.IsNotEmpty(list);
        }

        public IEnumerable<TSingle> ResourceFixtureAll()
        {
            return ResourceClient.All();
        }

        [SetUp]
        public void Setup()
        {
            SetupClient();
        }
    }
}
