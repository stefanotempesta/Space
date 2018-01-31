using Microsoft.Azure.Relay;
using MsdnDataIntegrationPatterns.DataIntegration.Entities;
using MsdnDataIntegrationPatterns.DataIntegration.External;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MsdnDataIntegrationPatterns.DataIntegration
{
    public class Relay
    {
        public async Task ExecuteAsync(ParcelTrackingEntity parcel)
        {
            GisObject gisObject = new GisObject
            {
                Latitude = parcel.Latitude,
                Longitude = parcel.Longitude
            };

            // Create a new hybrid connection client
            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(keyName, key);
            var client = new HybridConnectionClient(new Uri($"sb://{relayNamespace}/{connectionName}"), tokenProvider);

            // Initiate the connection
            var relayConnection = await client.CreateConnectionAsync();

            // Bi-directional sync of GIS data:
            //   1. Send a GIS object to the relay
            //   2. Receive a GIS object with the resolved address and update the entity
            //   3. Close the relay connection
            await new Task(
                () => SendToRelay(relayConnection, gisObject)
                .ContinueWith(async (t) =>
                {
                    GisObject resolved = await ReadFromRelay(relayConnection);
                    ShowAddress(resolved);
                })
                .ContinueWith(async (t) => await relayConnection.CloseAsync(CancellationToken.None))
                .Start());

            void ShowAddress(GisObject resolved)
            {
                var addr = resolved.Address;
                parcel.Address = $"{addr.Line}, {addr.ZipCode} {addr.City}, {addr.Country}";
                Update(parcel); // Update the tracking information in the system
            }
        }

        private async Task SendToRelay(HybridConnectionStream relayConnection, GisObject gisObject)
        {
            // Write the GIS object to the hybrid connection
            var writer = new StreamWriter(relayConnection) { AutoFlush = true };
            string message = JsonConvert.SerializeObject(gisObject);
            await writer.WriteAsync(message);
        }

        private async Task<GisObject> ReadFromRelay(HybridConnectionStream relayConnection)
        {
            // Read the GIS object from the hybrid connection
            var reader = new StreamReader(relayConnection);
            string message = await reader.ReadToEndAsync();
            GisObject gisObject = JsonConvert.DeserializeObject<GisObject>(message);

            return gisObject;
        }

        private void Update(ParcelTrackingEntity parcel)
        {
            throw new NotImplementedException();
        }

        private readonly string relayNamespace = "<RelayNamespace>.servicebus.windows.net";
        private readonly string connectionName = "<HybridConnectionName>";
        private readonly string keyName = "<KeyName>";
        private readonly string key = "<Key>";
    }
}
