using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiometricSecurity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessController : ControllerBase
    {
        [HttpPost]
        [Authorize(Policy = "AuthorizedUser")]
        public IActionResult Post([FromBody] string siteId)
        {
            var response = new
            {
                User = HttpContext.User.Identity.Name,
                SiteId = siteId
            };

            return new JsonResult(response);
        }
    }
}
