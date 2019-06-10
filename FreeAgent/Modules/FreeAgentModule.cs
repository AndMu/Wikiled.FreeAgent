using Autofac;
using System;

namespace Wikiled.FreeAgent.Modules
{
    public class FreeAgentModule : Module
    {
        private readonly string apiKey;

        private readonly string appSecret;

        public FreeAgentModule(string apiKey, string appSecret)
        {
            this.apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            this.appSecret = appSecret ?? throw new ArgumentNullException(nameof(appSecret));
        }
    }
}
