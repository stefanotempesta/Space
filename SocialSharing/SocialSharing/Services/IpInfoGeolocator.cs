using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialSharing.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SocialSharing.Services
{
    public class IpInfoGeolocator : IGeolocator
    {
        public IpInfoGeolocator(string geolocationApiUrl)
        {
            GeolocationApiUrl = geolocationApiUrl;
        }

        public string GeolocationApiUrl { get; }

        public async Task<GeolocationInfo> Geolocate(string ipAddress)
        {
            using (var httpClient = new HttpClient { BaseAddress = new Uri(GeolocationApiUrl) })
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await httpClient.GetAsync($"/{ipAddress}/geo");
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadAsAsync<GeolocationInfo>();
            }
        }
    }
}
