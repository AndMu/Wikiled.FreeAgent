using RestSharp;
using Wikiled.FreeAgent.Helpers;

namespace Wikiled.FreeAgent.Client
{
    public abstract class BaseClient
    {
        public FreeAgentClient Client;

        public BaseClient(FreeAgentClient client)
        {
            Client = client;
        }

        public virtual string ResourceName => "unknown";

        public RequestHelper Helper => Client.Helper;

        public string Version => FreeAgentClient.Version;

        public virtual RestRequest CreateBasicRequest(Method method, string appendToUrl = "", string resourceOverride = null)
        {
            var request = new RestRequest(method);
            request.Resource = "v{version}/{resource}" + appendToUrl;
            request.AddParameter("version", Version, ParameterType.UrlSegment);
            request.AddParameter("resource",
                                 string.IsNullOrEmpty(resourceOverride) ? ResourceName : resourceOverride,
                                 ParameterType.UrlSegment);

            SetAuthentication(request);

            return request;
        }

        public virtual void CustomizeAllRequest(RestRequest request)
        {
            //
        }

        protected void SetAuthentication(RestRequest request)
        {
            request.AddHeader("Authorization", "Bearer " + Client.CurrentAccessToken.AccessToken);
        }
    }
}
