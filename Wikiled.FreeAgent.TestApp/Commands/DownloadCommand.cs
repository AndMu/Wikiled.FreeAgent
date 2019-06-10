using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Wikiled.Console.Arguments;

namespace Wikiled.FreeAgent.TestApp.Commands
{
    public class DownloadCommand : Command
    {
        public DownloadCommand(ILogger logger)
            : base(logger)
        {
        }

        protected override Task Execute(CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
