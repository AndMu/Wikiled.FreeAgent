using Autofac;
using System.ComponentModel.DataAnnotations;
using Wikiled.Console.Arguments;
using Wikiled.FreeAgent.Modules;

namespace Wikiled.FreeAgent.TestApp.Commands.Config
{
    public class DownloadConfig : ICommandConfig
    {
        [Required]
        public string AppKey { get; set; }

        [Required]
        public string AppSecret { get; set; }

        [Required]
        public string Out { get; set; }

        public void Build(ContainerBuilder builder)
        {
            builder.RegisterModule(new FreeAgentModule(AppKey, AppSecret));
        }
    }
}
