using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceLibrary
{
    public class Thermostat
    {
        public Thermostat(string deviceId)
        {
            _deviceId = deviceId;
        }

        public string Id => _deviceId;

        public delegate void NewMeasureEventHandler(Thermostat sender, NewMeasureEventArgs e);

        public event NewMeasureEventHandler NewMeasure;

        protected virtual void OnNewMeasure(NewMeasureEventArgs e)
        {
            NewMeasure?.Invoke(this, e);
        }

        public async Task<float> ReadTemperatureAsync(CancellationToken ct)
        {
            float temperature = await Task.Run<float>(async () =>
            {
                // Simulate measuring temperature
                float baseTemperature = 10.0F;
                Random rand = new Random();
                float newTemperature = baseTemperature + (float)rand.NextDouble() * 4 - 2;

                await Task.Delay(1000, ct);
                ct.ThrowIfCancellationRequested();

                return newTemperature;
            }, ct);

            NewMeasureEventArgs eventArgs = new NewMeasureEventArgs
            {
                Measure = temperature,
                MeasuredAt = DateTime.Now
            };

            OnNewMeasure(eventArgs);

            return temperature;
        }

        private string _deviceId;
    }
}
