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
    public class BodyRequirementHandler : AuthorizationHandler<BodyRecognitionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BodyRecognitionRequirement requirement)
        {
            string siteId = (context.Resource as HttpContext).Request.Query["siteId"];
            IRecognition recognizer = new BodyRecognition();

            if (recognizer.Recognize(siteId, out string name) >= requirement.ConfidenceScore)
            {
                context.User.AddIdentity(new ClaimsIdentity(new GenericIdentity(name)));
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
