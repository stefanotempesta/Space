using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialSharing.Models
{
    [Table("AuditTrail")]
    public class AuditTrailEntry
    {
        public int Id { get; set; }

        public DateTime TimeStamp { get; set; }

        public string Browser { get; set; }

        public string BrowserVersion { get; set; }

        public int BrowserMajorVersion { get; set; }

        public bool BrowserIsMobileDevice { get; set; }

        public string BrowserPlatform { get; set; }

        public string UrlReferrer { get; set; }

        public string UserAgent { get; set; }

        public string UserHostAddress { get; set; }

        public string UserCountry { get; set; }

        public string SocialName { get; set; }

        public string SocialAction { get; set; }

        public string ModelType { get; set; }

        public int ModelId { get; set; }
    }
}
