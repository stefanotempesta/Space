using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiometricSecurity.Api.Security
{
    public class FaceRecognitionRequirement : IAuthorizationRequirement
    {
        public double ConfidenceScore { get; }

        public FaceRecognitionRequirement(double confidence) => ConfidenceScore = confidence;
    }
}
