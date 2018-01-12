using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GisSpatial
{
    public class GeoCoordinates
    {
        public GeoCoordinates() : this(0.0M, 0.0M)
        {
        }

        public GeoCoordinates(decimal latitude, decimal longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public const string NMEA0183 = "NMEA0183";
        public const string WGS84 = "WGS84";

        private decimal _latitude;
        public decimal Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                if (value < -90.0M || value > 90.0M)
                {
                    throw new ArgumentOutOfRangeException("Latitude must be between -90.0 and 90.0.");
                }

                _latitude = value;
            }
        }

        private decimal _longitude;
        public decimal Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                if (value < -180.0M || value > 180.0M)
                {
                    throw new ArgumentOutOfRangeException("Longitude must be between -180.0 and 180.0.");
                }

                _longitude = value;
            }
        }

        public override string ToString()
        {
            return ToString("D");
        }

        public string ToString(string format)
        {
            switch (format)
            {
                case NMEA0183:
                    return FormatNMEA0183();

                case WGS84:
                    return FormatWGS84();

                default:
                    throw new ArgumentOutOfRangeException("Invalid format specified.", nameof(format));
            }
        }

        public static GeoCoordinates FromNMEA0183(string degrees)
        {
            if (string.IsNullOrEmpty(degrees))
            {
                throw new ArgumentNullException(nameof(degrees));
            }

            // Latitude, Longitude - e.g. 5321.5802 N, 00630.3372 W
            string[] coords = degrees.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (coords.Length < 2)
            {
                throw new ArgumentException("Invalid degree coordinates.", nameof(degrees));
            }

            return new GeoCoordinates
            {
                Latitude = ParseLatitude(coords[0].Trim()),
                Longitude = ParseLongitude(coords[1].Trim())
            };
        }

        private string FormatNMEA0183()
        {
            // Latitude,Longitude - e.g. 5321.5802 N, 00630.3372 W
            string latitude = FormatLatitudeDegrees(this.Latitude);
            string longitude = FormatLongitudeDegrees(this.Longitude);

            return $"{latitude}, {longitude}";
        }

        private string FormatWGS84()
        {
            // Latitude,Longitude - e.g. 53.359686, 6.505620
            return $"{this.Latitude}, {this.Longitude}";
        }

        private string FormatLatitudeDegrees(decimal latitude)
        {
            // Latitude: DDMM.mmmm - e.g. 53.359686 => 5321.5802 N
            string sign = latitude > 0 ? "N" : "S";

            latitude = Math.Abs(latitude);
            string dd = decimal.Truncate(latitude).ToString("0#");
            string mm = (decimal.Subtract(latitude, decimal.Truncate(latitude)) * 60).ToString("0#.0000");

            return $"{dd}{mm} {sign}";
        }

        private string FormatLongitudeDegrees(decimal longitude)
        {
            // Longitude: DDDMM.mmmm - e.g. -6.505620 => 00630.3372 W
            string sign = longitude > 0 ? "E" : "W";

            longitude = Math.Abs(longitude);
            string dd = decimal.Truncate(longitude).ToString("00#");
            string mm = (decimal.Subtract(longitude, decimal.Truncate(longitude)) * 60).ToString("0#.0000");

            return $"{dd}{mm} {sign}";
        }

        private static decimal ParseLatitude(string coords)
        {
            // Latitude: DDMM.mmmm - e.g. 5321.5802 N
            if (!coords.EndsWith("N", StringComparison.OrdinalIgnoreCase) && !coords.EndsWith("S", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Latitude coordinate not found.");
            }

            if (coords.Length < 4)
            {
                throw new ArgumentException("Invalid latitude format.");
            }

            int dd = 0;
            try
            {
                dd = int.Parse(coords.Substring(0, 2));
                if (dd > 90)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            catch when (dd > 90)
            {
                throw new ArgumentOutOfRangeException("Degrees in latitude cannot exceed 90.");
            }
            catch
            {
                throw new ArgumentException("Invalid degrees format in latitude.");
            }

            double mm = 0.0D;
            try
            {
                string minutes = Regex.Match(coords.Substring(2), @"(\d+).(\d+)").Value;
                mm = double.Parse(minutes);
                if ((dd == 90 && mm > 0.0D) || mm >= 60.0D)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            catch when (dd == 90 && mm > 0.0D)
            {
                throw new ArgumentOutOfRangeException("Degrees in latitude cannot exceed 90.");
            }
            catch when (mm >= 60.0D)
            {
                throw new ArgumentOutOfRangeException("Minutes in latitude cannot exceed 60.");
            }
            catch
            {
                throw new ArgumentException("Invalid minutes format in latitude.");
            }

            decimal latitude = Convert.ToDecimal(dd + mm / 60);
            if (coords.EndsWith("S", StringComparison.OrdinalIgnoreCase))
            {
                latitude = decimal.Negate(latitude);
            }

            return latitude;
        }

        private static decimal ParseLongitude(string coords)
        {
            // Longitude: DDDMM.mmmm - e.g. 00630.3372 W
            if (!coords.EndsWith("W", StringComparison.OrdinalIgnoreCase) && !coords.EndsWith("E", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Longitude coordinate not found.");
            }

            if (coords.Length < 5)
            {
                throw new ArgumentException("Invalid longitude format.");
            }

            int dd = 0;
            try
            {
                dd = int.Parse(coords.Substring(0, 3));
                if (dd > 180)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            catch when (dd > 180)
            {
                throw new ArgumentOutOfRangeException("Degrees in longitude cannot exceed 180.");
            }
            catch
            {
                throw new ArgumentException("Invalid degrees format in longitude.");
            }

            double mm = 0.0D;
            try
            {
                string minutes = Regex.Match(coords.Substring(3), @"(\d+).(\d+)").Value;
                mm = double.Parse(minutes);
                if ((dd == 180 && mm > 0.0D) || mm >= 60.0D)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            catch when (dd == 180 && mm > 0.0D)
            {
                throw new ArgumentOutOfRangeException("Degrees in longitude cannot exceed 180.");
            }
            catch when (mm >= 60.0D)
            {
                throw new ArgumentOutOfRangeException("Minutes in longitude should be less than 60.");
            }
            catch
            {
                throw new ArgumentException("Invalid minutes format in longitude.");
            }

            decimal longitude = Convert.ToDecimal(dd + mm / 60);
            if (coords.EndsWith("W", StringComparison.OrdinalIgnoreCase))
            {
                longitude = decimal.Negate(longitude);
            }

            return longitude;
        }
    }
}
