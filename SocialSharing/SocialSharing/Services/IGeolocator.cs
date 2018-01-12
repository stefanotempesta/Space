using SocialSharing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialSharing.Services
{
    public interface IGeolocator
    {
        Task<GeolocationInfo> Geolocate(string ipAddress);
    }
}
