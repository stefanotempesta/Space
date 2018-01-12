using DeviceLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Simulator
{
    class ThermostatSimulator
    {
        public async Task RunSimulation()
        {
            string iotHubHostName = ConfigurationManager.AppSettings["AzureIoTHub-HostName"];
            string accessPolicyName = ConfigurationManager.AppSettings["AzureIoTHub-AccessPolicyName"];
            string accessPrimaryKey = ConfigurationManager.AppSettings["AzureIoTHub-AccessPrimaryKey"];
            ThermostatManager manager = new ThermostatManager(iotHubHostName, accessPolicyName, accessPrimaryKey);

            Thermostat thermostat1 = new Thermostat("THERMO-001");
            Thermostat thermostat2 = new Thermostat("THERMO-002");

            await manager.AddDeviceAsync(thermostat1, 10000);   // Run for 10 seconds
            await manager.AddDeviceAsync(thermostat2, 15000);   // Run for 15 seconds

            thermostat1.NewMeasure += (s, e) => DeviceWrite(s, e);
            thermostat2.NewMeasure += (s, e) => DeviceWrite(s, e);

            manager.ReadCanceled += (s, e) => DeviceCanceled(s);
            manager.Run();
        }

        void DeviceWrite(Thermostat s, NewMeasureEventArgs e)
        {
            Console.ResetColor();
            Console.WriteLine($"Device '{s.Id}' measured temperature '{e.Measure}' at {e.MeasuredAt.ToLongTimeString()}");
        }

        void DeviceCanceled(Thermostat s)
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Device '{s.Id}' canceled at {DateTime.Now.ToLongTimeString()}");
        }
    }
}
