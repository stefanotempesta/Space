using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GisSpatial
{
    class Program
    {
        static void Main(string[] args)
        {
            string degrees = "5321.5802 N, 00630.3372 W";
            Console.WriteLine("Coordinates: {0}", degrees);

            GeoCoordinates coords = GeoCoordinates.FromNMEA0183(degrees);

            Console.WriteLine("NMEA0183: {0}", coords.ToString(GeoCoordinates.NMEA0183));
            Console.WriteLine("WGS84: {0}", coords.ToString(GeoCoordinates.WGS84));
        }
    }
}
