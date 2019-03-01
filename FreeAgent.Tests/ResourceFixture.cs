using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Wikiled.FreeAgent.Client;
using Wikiled.FreeAgent.Extensions;
using Wikiled.FreeAgent.Models;
using Task = System.Threading.Tasks.Task;

namespace Wikiled.FreeAgent.Tests
{
    public class ResourceFixture<TSingleWrapper, TListWrapper, TSingle> : BaseFixture
        where TSingle : BaseModel
        where TListWrapper : new()
        where TSingleWrapper : new()
    {
        protected bool ExecuteCanGetList = true, ExecuteCanGetListWithContent = true, ExecuteCanLoadById = true, ExecuteCanCreateSingle = true, ExecuteCanDeleteAndCleanup = true;

        protected Func<IObservable<TSingle>> GetAll;

        public virtual ResourceClient<TSingleWrapper, TListWrapper, TSingle> ResourceClient => throw new NotImplementedException("oops!");

        public virtual Task<bool> CanDelete(TSingle item)
        {
            return Task.FromResult(false);
        }

        public virtual void CheckSingleItem(TSingle item)
        {
        }

        public virtual void CompareSingleItem(TSingle originalItem, TSingle newItem)
        {
            throw new NotImplementedException("needs to be overridden");
        }

        public virtual Task<TSingle> CreateSingleItemForInsert()
        {
            throw new NotImplementedException("needs to be overridden");
        }

        public override async Task SetupClient()
        {
            await base.SetupClient().ConfigureAwait(false);
            GetAll = ResourceFixtureAll;
        }

        [Test]
        public async Task CanCreateSingle()
        {
            if (!ExecuteCanCreateSingle)
            {
                Assert.Ignore("ExecuteCanCreateSingle is being ignored");
                return;
            }

            TSingle item = await CreateSingleItemForInsert().ConfigureAwait(false);

            TSingle result = await ResourceClient.Put(item).ConfigureAwait(false);

            CompareSingleItem(item, result);
        }

        [Test]
        public async Task CanDeleteAndCleanup()
        {
            if (!ExecuteCanDeleteAndCleanup)
            {
                Assert.Ignore("ExecuteCanDeleteAndCleanup is being ignored");
                return;
            }

            var items = await GetAll().ToArray();
            CheckAllList(items);
            foreach (var item in items)
            {
                if (!await CanDelete(item).ConfigureAwait(false))
                {
                    continue;
                }

                ResourceClient.Delete(item.Id());
                var deletedClient = ResourceClient.Get(item.Id());
                Assert.IsNull(deletedClient);
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
        public async Task CanGetListWithContent()
        {
            if (!ExecuteCanGetListWithContent)
            {
                Assert.Ignore("ExecuteCanGetListWithContent is being ignored");
                return;
            }

            var list = await GetAll().ToArray();

            CheckAllList(list);

            foreach (var item in list)
            {
                CheckSingleItem(item);
            }
        }

        [Test]
        public async Task CanLoadById()
        {
            if (!ExecuteCanLoadById)
            {
                Assert.Ignore("ExecuteCanLoadById is being ignored");
                return;
            }

            var items = await GetAll().ToArray();
            CheckAllList(items);

            foreach (var item in items)
            {
                var newItem = await ResourceClient.Get(item.Id()).ConfigureAwait(false);
                CompareSingleItem(item, newItem);
            }
        }

        public void CheckAllList(IEnumerable<TSingle> list)
        {
            Assert.IsNotNull(list);
            Assert.IsNotEmpty(list);
        }

        public IObservable<TSingle> ResourceFixtureAll()
        {
            return ResourceClient.All();
        }

        [SetUp]
        public async Task Setup()
        {
            await SetupClient().ConfigureAwait(false);
        }
    }
}
