using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using Wikiled.FreeAgent.Exceptions;

namespace Wikiled.FreeAgent.Extensions
{
    public static class RestClientExtensions
    {
        public static Task<TResult> ExecuteTask<TResult>(this IRestClient client, IRestRequest request) where TResult : new()
        {
            var tcs = new TaskCompletionSource<TResult>();
            WaitCallback
                asyncWork = _ =>
                {
                    try
                    {
                        client.ExecuteAsync<TResult>(
                            request,
                            (response, asynchandler) =>
                            {
                                if (response.StatusCode != HttpStatusCode.OK)
                                {
                                    tcs.SetException(new FreeAgentException(response));
                                }
                                else
                                {
                                    tcs.SetResult(response.Data);
                                }
                            });
                    }
                    catch (Exception exc)
                    {
                        tcs.SetException(exc);
                    }
                };

            return ExecuteTask(asyncWork, tcs);
        }

        public static Task<IRestResponse> ExecuteTask(this IRestClient client, IRestRequest request)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();

            WaitCallback
                asyncWork = _ =>
                {
                    try
                    {
                        client.ExecuteAsync(
                            request,
                            (response, asynchandler) =>
                            {
                                if (response.StatusCode != HttpStatusCode.OK)
                                {
                                    tcs.SetException(new FreeAgentException(response));
                                }
                                else
                                {
                                    tcs.SetResult(response);
                                }
                            });
                    }
                    catch (Exception exc)
                    {
                        tcs.SetException(exc);
                    }
                };

            return ExecuteTask(               asyncWork, tcs);
        }

        private static Task<TResult> ExecuteTask<TResult>(WaitCallback asyncWork, TaskCompletionSource<TResult> tcs)
        {
            ThreadPool.QueueUserWorkItem(asyncWork);
            return tcs.Task;
        }
    }
}
