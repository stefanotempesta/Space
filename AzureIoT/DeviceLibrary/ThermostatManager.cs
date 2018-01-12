using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Common.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceLibrary
{
    public class ThermostatManager
    {
        public ThermostatManager(string hostName, string accessPolicyName, string accessPrimaryKey)
        {
            this.hostName = hostName;

            string connectionString = $"HostName={hostName};SharedAccessKeyName={accessPolicyName};SharedAccessKey={accessPrimaryKey}";
            registry = RegistryManager.CreateFromConnectionString(connectionString);

            deviceList = new List<ThermostatClient>();
        }

        protected string hostName { get; private set; }

        protected RegistryManager registry { get; private set; }

        protected IList<ThermostatClient> deviceList { get; set; }

        public async Task AddDeviceAsync(Thermostat thermostat, int runForMilliseconds = -1)
        {
            bool exists = deviceList.SingleOrDefault(d => d.Id == thermostat.Id) != null;
            if (exists)
            {
                return;
            }

            // Add the device to the IoT Hub
            Device device;
            try
            {
                device = await registry.AddDeviceAsync(new Device(thermostat.Id));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registry.GetDeviceAsync(thermostat.Id);
            }

            // Creates a device client for communication with the IoT Hub
            DeviceClient client = DeviceClient.Create(this.hostName, 
                new DeviceAuthenticationWithRegistrySymmetricKey(
                    device.Id, 
                    device.Authentication.SymmetricKey.PrimaryKey));
            
            // Register the event handler for when a new temperature is measured
            thermostat.NewMeasure += (s, e) =>
            {
                SendMessagesAsync(s, e);
            };

            ThermostatClient thermostatClient = new ThermostatClient
            {
                Thermostat = thermostat,
                Device = device,
                Client = client,
                RunForMilliseconds = runForMilliseconds
            };

            deviceList.Add(thermostatClient);
        }

        public async Task RemoveDeviceAsync(Thermostat thermostat)
        {
            ThermostatClient thermostatClient = deviceList.SingleOrDefault(d => d.Thermostat.Id == thermostat.Id);
            if (thermostatClient == null) return;

            deviceList.Remove(thermostatClient);

            await registry.RemoveDeviceAsync(thermostat.Id);
        }

        public void Run()
        {
            Parallel.ForEach(deviceList, async (item) =>
            {
                CancellationTokenSource cts = item.RunForMilliseconds < 0 ? null : new CancellationTokenSource(item.RunForMilliseconds);
                await RunThermostatAsync(item.Thermostat, cts);
            });
        }

        private async Task RunThermostatAsync(Thermostat thermostat, CancellationTokenSource cts)
        {
            try
            {
                while (true)
                {
                    await thermostat.ReadTemperatureAsync(cts != null ? cts.Token : CancellationToken.None);
                }
            }
            catch (OperationCanceledException)
            {
                OnReadCanceled(thermostat);
            }
        }

        public delegate void ReadCanceledEventHandler(Thermostat sender, EventArgs e);

        public event ReadCanceledEventHandler ReadCanceled;

        protected virtual void OnReadCanceled(Thermostat sender)
        {
            ReadCanceled?.Invoke(sender, EventArgs.Empty);
        }

        private async void SendMessagesAsync(Thermostat sender, NewMeasureEventArgs e)
        {
            ThermostatClient thermostatClient = deviceList.SingleOrDefault(d => d.Thermostat.Id == sender.Id);
            if (thermostatClient == null) return;

            var telemetryDataPoint = new
            {
                deviceId = sender.Id,
                temperature = e.Measure
            };

            var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
            var message = new Microsoft.Azure.Devices.Client.Message(Encoding.ASCII.GetBytes(messageString));

            await thermostatClient.Client.SendEventAsync(message);
        }
    }
}
