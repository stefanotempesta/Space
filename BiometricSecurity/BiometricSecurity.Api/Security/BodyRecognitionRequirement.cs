using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiometricSecurity.Api.Security
{
    public class BodyRecognitionRequirement : IAuthorizationRequirement
    {
        public double ConfidenceScore { get; }

        public BodyRecognitionRequirement(double confidence) => ConfidenceScore = confidence;
    }
}
