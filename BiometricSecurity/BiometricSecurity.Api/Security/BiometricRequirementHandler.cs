using BiometricSecurity.Recognition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace BiometricSecurity.Api.Security
{
    public class BiometricRequirementHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            string siteId = (context.Resource as HttpContext).Request.Query["siteId"];

            foreach (var requirement in context.PendingRequirements)
            {
                IRecognition recognizer = null;
                double score = 1.0d;

                switch (requirement)
                {
                    case FaceRecognitionRequirement r:
                        recognizer = new FaceRecognition();
                        score = r.ConfidenceScore;
                        break;
                    case BodyRecognitionRequirement r:
                        recognizer = new BodyRecognition();
                        score = r.ConfidenceScore;
                        break;
                    case VoiceRecognitionRequirement r:
                        recognizer = new VoiceRecognition();
                        score = r.ConfidenceScore;
                        break;
                }

                if (recognizer != null && recognizer.Recognize(siteId, out string name) >= score)
                {
                    context.User.AddIdentity(new ClaimsIdentity(new GenericIdentity(name)));
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }

            return Task.CompletedTask;
        }
    }
}
