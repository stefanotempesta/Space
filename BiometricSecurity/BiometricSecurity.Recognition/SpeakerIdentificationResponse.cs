using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace BiometricSecurity.Recognition
{
    public class SpeakerIdentificationResponse
    {
        public Guid IdentifiedProfileId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Confidence Confidence { get; set; }
    }
}
