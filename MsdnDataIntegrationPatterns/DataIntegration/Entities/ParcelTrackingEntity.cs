using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsdnDataIntegrationPatterns.DataIntegration.Entities
{
    public class ParcelTrackingEntity : Entity
    {
        public Guid OrderId { get; set; }

        public string TrackingNumber { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Address { get; set; }
    }
}
