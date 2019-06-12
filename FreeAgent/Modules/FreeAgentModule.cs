using Autofac;
using System;
using Wikiled.Common.Utilities.Auth;
using Wikiled.Common.Utilities.Auth.OAuth;
using Wikiled.FreeAgent.Auth;
using Wikiled.FreeAgent.Client;

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

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new AuthenticationData(apiKey, appSecret));
            builder.RegisterType<FreeAgentClient>().As<IFreeAgentClient>().As<IAuthClient<AccessTokenData>>().SingleInstance();
            builder.RegisterType<OAuthHelper>()
                .As<IOAuthHelper>()
                .OnActivating(ctx => ctx.Instance.RedirectUri = "http://localhost:9000/");

            builder.RegisterGeneric(typeof(OAuthAuthentication<>)).As(typeof(IAuthentication<>));
            builder.RegisterGenericDecorator(typeof(PersistedAuthentication<>), typeof(IAuthentication<>));
        }
    }
}
