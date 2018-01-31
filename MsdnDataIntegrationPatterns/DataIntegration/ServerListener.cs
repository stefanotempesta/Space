using Microsoft.Azure.Relay;
using MsdnDataIntegrationPatterns.DataIntegration.External;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MsdnDataIntegrationPatterns.DataIntegration
{
    class ServerListener
    {
        private async Task RunAsync()
        {
            // Create a new hybrid connection client
            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(keyName, key);
            var listener = new HybridConnectionListener(new Uri($"sb://{relayNamespace}/{connectionName}"), tokenProvider);

            // Open the listener and register it in Service Bus
            var cts = new CancellationTokenSource();
            await listener.OpenAsync(cts.Token);

            // Callback for cancellation token that will close the listener
            cts.Token.Register(() => listener.CloseAsync(CancellationToken.None));

            await Listen(listener, cts)
                .ContinueWith((t) => listener.CloseAsync(cts.Token));
        }

        private async Task Listen(HybridConnectionListener listener, CancellationTokenSource cts)
        {
            // Accept the next available, pending connection request
            HybridConnectionStream relayConnection;
            do
            {
                relayConnection = await listener.AcceptConnectionAsync();
                if (relayConnection != null)
                {
                    ProcessMessage(relayConnection, cts);
                }
            } while (relayConnection != null);
        }

        private async void ProcessMessage(HybridConnectionStream relayConnection, CancellationTokenSource cts)
        {
            // Bi-directional streams for read and write to the relay
            var reader = new StreamReader(relayConnection);
            var writer = new StreamWriter(relayConnection) { AutoFlush = true };
            while (!cts.IsCancellationRequested)
            {
                // Read a message in input from the relay
                var message = await reader.ReadToEndAsync();

                // Resolve address by invoking a service on the GIS server
                GisObject gisObject = JsonConvert.DeserializeObject<GisObject>(message);
                await new GisServer().ResolveAddressAsync(gisObject);

                // Write the message back to the relay
                message = JsonConvert.SerializeObject(gisObject);
                await writer.WriteLineAsync(message);
            }

            await relayConnection.CloseAsync(cts.Token);
        }

        private readonly string relayNamespace = "<RelayNamespace>.servicebus.windows.net";
        private readonly string connectionName = "<HybridConnectionName>";
        private readonly string keyName = "<KeyName>";
        private readonly string key = "<Key>";
    }
}
