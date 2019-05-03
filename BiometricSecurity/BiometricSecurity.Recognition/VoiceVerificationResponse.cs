using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace BiometricSecurity.Recognition
{
    public class VoiceVerificationResponse
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Result Result { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Confidence Confidence { get; set; }

        public string Phrase { get; set; }
    }

    public enum Result
    {
        Accept,
        Reject
    }

    public enum Confidence
    {
        Low = 1,
        Normal = 50,
        High = 99
    }
}
