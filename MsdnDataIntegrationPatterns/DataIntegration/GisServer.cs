using MsdnDataIntegrationPatterns.DataIntegration.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsdnDataIntegrationPatterns.DataIntegration
{
    class GisServer
    {
        public async Task ResolveAddressAsync(GisObject gisObject)
        {
            // Do reverse geolocation from coordinates to address
            // ...

            gisObject.Address = new Address
            {
                Line = "<AddressLine>",
                City = "<City>",
                ZipCode = "<ZipCode>",
                Country = "<Country>"
            };
        }
    }
}
