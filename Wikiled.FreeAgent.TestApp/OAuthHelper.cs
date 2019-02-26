using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wikiled.Common.Utilities.Helpers;

namespace Wikiled.FreeAgent.TestApp
{
    public class OAuthHelper : IDisposable
    {
        private readonly ILogger<OAuthHelper> logger;

        private readonly HttpListener http = new HttpListener();

        public OAuthHelper(ILogger<OAuthHelper> logger)
        {
            this.logger = logger;
        }

        public string RedirectUri { get; private set; }

        public string Code { get; private set; }

        public void StartService()
        {
            // Creates a redirect URI using an available port on the loopback address.
            RedirectUri = $"http://{IPAddress.Loopback}:{GetRandomUnusedPort()}/";
            logger.LogInformation("redirect URI: " + RedirectUri);

            // Creates an HttpListener to listen for requests on that redirect URI.
            http.Prefixes.Add(RedirectUri);
            logger.LogInformation("Listening...");
            http.Start();
        }

        public async Task Start(string serviceUrl)
        {
            // Opens request in the browser.
            ExternaApp.OpenUrl(serviceUrl);

            // Waits for the OAuth authorization response.
            var context = await http.GetContextAsync().ConfigureAwait(false);

            // Sends an HTTP response to the browser.
            var response = context.Response;
            string responseString = "<html><head><meta http-equiv='refresh' content='10;url=https://google.com'></head><body>Please return to the app.</body></html>";
            var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var responseOutput = response.OutputStream;
            Task responseTask = responseOutput.WriteAsync(buffer, 0, buffer.Length)
                                              .ContinueWith(
                                                  task =>
                                                  {
                                                      responseOutput.Close();
                                                      http.Stop();
                                                      logger.LogInformation("HTTP server stopped.");
                                                  });

            // Checks for errors.
            if (context.Request.QueryString.Get("error") != null)
            {
                logger.LogInformation(($"OAuth authorization error: {context.Request.QueryString.Get("error")}"));
                return;
            }
            if (context.Request.QueryString.Get("code") == null || context.Request.QueryString.Get("state") == null)
            {
                logger.LogInformation("Malformed authorization response. " + context.Request.QueryString);
                return;
            }

            // extracts the code
            var code = context.Request.QueryString.Get("code");
            var incomingState = context.Request.QueryString.Get("state");

            // Compares the receieved state to the expected value, to ensure that
            // this app made the request which resulted in authorization.
            //if (incomingState != state)
            //{
            //    logger.LogInformation($"Received request with invalid state ({incomingState})");
            //    return;
            //}

            logger.LogInformation("Authorization code: " + code);
            Code = code;
        }

        private static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }


        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private void BringConsoleToFront()
        {
            SetForegroundWindow(GetConsoleWindow());
        }

        public void Dispose()
        {
            http.Close();
        }
    }
}
