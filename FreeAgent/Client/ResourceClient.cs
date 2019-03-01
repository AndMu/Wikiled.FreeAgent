using System;
using System.Collections.Generic;
using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;
using RestSharp;
using Wikiled.FreeAgent.Exceptions;
using Wikiled.FreeAgent.Extensions;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Client
{
    public abstract class ResourceClient<TSingleWrapper, TListWrapper, TSingle> : BaseClient
        where TSingle : BaseModel
        where TListWrapper : new()
        where TSingleWrapper : new()
    {
        private const int PageSize = 50;

        protected ResourceClient(FreeAgentClient client)
            : base(client)
        {
        }

        public abstract List<TSingle> ListFromWrapper(TListWrapper wrapper);

        public abstract TSingle SingleFromWrapper(TSingleWrapper wrapper);

        public abstract TSingleWrapper WrapperFromSingle(TSingle single);

        public virtual void AddPaging(RestRequest request, int page = 1)
        {
            request.AddParameter("page", page, ParameterType.GetOrPost);
            request.AddParameter("per_page", PageSize, ParameterType.GetOrPost);
        }

        public IObservable<TSingle> All(Action<RestRequest> customizeRequest = null)
        {
            return Observable.Create<TSingle>(
                async observer =>
                {
                    int page = 1;
                    while (true)
                    {
                        var request = CreateAllRequest();
                        customizeRequest?.Invoke(request);

                        AddPaging(request, page);

                        var response = await Client.Execute<TListWrapper>(request).ConfigureAwait(false);
                        if (response != null)
                        {
                            var newItems = ListFromWrapper(response);
                            foreach (var item in newItems)
                            {
                                observer.OnNext(item);
                            }

                            if (newItems.Count < PageSize)
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }

                        page++;
                    }

                    observer.OnCompleted();
                });
        }

        public void Delete(string id)
        {
            var request = CreateDeleteRequest(id);
            var response = Client.Execute(request);
        }

        public async Task<TSingle> Get(string id)
        {
            try
            {
                var request = CreateGetRequest(id);
                var response = await Client.Execute<TSingleWrapper>(request).ConfigureAwait(false);
                return response != null ? SingleFromWrapper(response) : null;
            }
            catch (FreeAgentException fex)
            {
                if (fex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }
        }

        public async Task<TSingle> Put(TSingle c)
        {
            var request = CreatePutRequest(c);
            var response = await Client.Execute<TSingleWrapper>(request).ConfigureAwait(false);
            return response != null ? SingleFromWrapper(response) : null;
        }

        protected RestRequest CreateAllRequest()
        {
            var request = CreateBasicRequest(Method.GET);
            CustomizeAllRequest(request);

            return request;
        }

        protected RestRequest CreateDeleteRequest(string id)
        {
            var request = CreateBasicRequest(Method.DELETE, "/{id}");

            request.AddParameter("id", id, ParameterType.UrlSegment);

            return request;
        }

        protected RestRequest CreateGetRequest(string id)
        {
            var request = CreateBasicRequest(Method.GET, "/{id}");
            request.AddParameter("id", id, ParameterType.UrlSegment);

            return request;
        }

        protected RestRequest CreatePutRequest(TSingle item)
        {
            bool isNewRecord = string.IsNullOrEmpty(item.url);
            var request = CreateBasicRequest(isNewRecord ? Method.POST : Method.PUT, isNewRecord ? "" : "/{id}");

            if (item is IRemoveUrlOnSerialization || item is IRemoveRecurringOnSerialization)
            {
                request.JsonSerializer = new UrlParsingJsonSerializer();
            }

            request.RequestFormat = DataFormat.Json;

            if (!isNewRecord) request.AddParameter("id", item.Id(), ParameterType.UrlSegment);
            request.AddJsonBody(WrapperFromSingle(item));

            return request;
        }
    }
}
