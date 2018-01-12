using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceLibrary
{
    public class ThermostatClient
    {
        public string Id => Device.Id;

        public string Key => Device.Authentication.SymmetricKey.PrimaryKey;

        public int RunForMilliseconds { get; set; }

        public Thermostat Thermostat { get; internal set; }

        public Device Device { get; internal set; }

        public DeviceClient Client { get; internal set; }
    }
}
