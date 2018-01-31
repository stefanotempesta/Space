using System;
using System.Collections.Generic;

namespace MsdnDataIntegrationPatterns.DataIntegration.External
{
    class GisObject
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public Address Address { get; set; }
    }

    public struct Address
    {
        public string Line { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public string Country { get; set; }
    }
}
