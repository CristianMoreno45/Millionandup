
using Microsoft.AspNetCore.Mvc;

namespace Millionandup.MsIdentityServer.Controllers
{

    /// <summary>
    ///  Health Check: you can see on https://BaseUrl/ping
    /// </summary>
    [ApiController]
    [Route("")]
    public abstract class HealthCheckController : ControllerBase
    {
        /// <summary>
        /// Health Check
        /// </summary>
        /// <returns>OK</returns>
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new { statusCode = 200, message = "OK" });
        }
    }
}
