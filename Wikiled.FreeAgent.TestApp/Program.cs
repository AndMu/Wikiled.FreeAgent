using NLog.Extensions.Logging;
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wikiled.Common.Logging;
using Wikiled.Console.Arguments;
using Wikiled.FreeAgent.TestApp.Commands;
using Wikiled.FreeAgent.TestApp.Commands.Config;

namespace Wikiled.FreeAgent.TestApp
{
    public class Program
    {
        private static Task task;

        private static CancellationTokenSource source;

        private static AutoStarter starter;

        static async Task Main(string[] args)
        {
            NLog.LogManager.LoadConfiguration("nlog.config");
            starter = new AutoStarter(ApplicationLogging.LoggerFactory, "FreeAgent App", args);
            starter.LoggerFactory.AddNLog();
            starter.RegisterCommand<DownloadCommand, DownloadConfig>("download");
            source = new CancellationTokenSource();
            task = starter.StartAsync(source.Token);
            System.Console.WriteLine("Please press CTRL+C to break...");
            System.Console.CancelKeyPress += ConsoleOnCancelKeyPress;
            await starter.Status.LastOrDefaultAsync();
            System.Console.WriteLine("Exiting...");
        }

        private static async void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            if (!task.IsCompleted)
            {
                source.Cancel();
            }

            source = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await starter.StopAsync(source.Token).ConfigureAwait(false);
        }
    }
}
