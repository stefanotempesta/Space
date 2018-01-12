using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceLibrary
{
    public class NewMeasureEventArgs : EventArgs
    {
        public float Measure { get; set; }

        public DateTime MeasuredAt { get; set; }
    }
}
