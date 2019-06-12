using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wikiled.Common.Extensions;
using Wikiled.Common.Utilities.Auth;
using Wikiled.Console.Arguments;
using Wikiled.FreeAgent.Auth;
using Wikiled.FreeAgent.Client;
using Wikiled.FreeAgent.Extensions;
using Wikiled.FreeAgent.Models;
using Wikiled.FreeAgent.TestApp.Commands.Config;
using Task = System.Threading.Tasks.Task;

namespace Wikiled.FreeAgent.TestApp.Commands
{
    public class DownloadCommand : Command
    {
        private readonly IAuthentication<AccessTokenData> auth;

        private readonly ILogger<DownloadCommand> logger;

        private readonly IFreeAgentClient client;

        private DownloadConfig config;

        public DownloadCommand(ILogger<DownloadCommand> logger, IFreeAgentClient client, IAuthentication<AccessTokenData> auth, DownloadConfig config)
            : base(logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.auth = auth ?? throw new ArgumentNullException(nameof(auth));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        protected override async Task Execute(CancellationToken token)
        {
            logger.LogInformation("Authenticating...");
            var result = await auth.Authenticate().ConfigureAwait(false);
            var banks = await client.BankAccount.All().ToArray();
            foreach (var bank in banks)
            {
                var path = Path.Combine(config.Out, bank.Id());
                path.EnsureDirectoryExistence();
                logger.LogInformation("Download for the bank {0}", bank.name);
                await client.BankTransactionExplanation.All(bank)
                    .Where(item => item.Attachment != null)
                    .Select(ProcessBankTransaction)
                    .Merge()
                    .LastAsync();
            }
        }

        private async Task<BankTransactionExplanation> ProcessBankTransaction(BankTransactionExplanation transaction)
        {
            try
            {
                var request = WebRequest.Create(new Uri(transaction.Attachment.ContentSrc));
                var transactionId = transaction.BankTransaction.LocalId();
                var bankId = transaction.BankAccount.LocalId();
                var dateTime = transaction.DatedOn.FromModelDate(); ;
                var dateStr = dateTime.ToString("yyyy MMMM dd");
                logger.LogDebug("Downloading: {1} {0}", transactionId, dateTime);

                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (var file = new FileStream($@"{config.Out}\{bankId}\{dateStr}_{transactionId}_file.pdf", FileMode.Create, FileAccess.Write))
                        {
                            stream.CopyTo(file);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                logger.LogError(e, "Download failed");
            }

            return transaction;
        }
    }
}
