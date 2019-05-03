using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BiometricSecurity.Recognition
{
    public class Profile
    {
        [JsonProperty("identificationProfileId")]
        public Guid ProfileId { get; set; }

        public string Name { get; set; }
    }
}
