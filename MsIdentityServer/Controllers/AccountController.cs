using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Millionandup.Framework.Exceptions;
using Millionandup.MsIdentityServer.AggregatesModel;
using Millionandup.MsIdentityServer.DTO;
using static IdentityServer4.IdentityServerConstants;

namespace Millionandup.MsIdentityServer.Controllers
{
    /// <summary>
    /// API Responsible for managing the creation and validation of clients (users) in IS4
    /// </summary>
    [ApiController]
    [Route("Account")]
    public class AccountController : ControllerBase
    {

        private readonly ILogger _logger;
        private readonly IAccount _account;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="logger">[Dependency injection] logger</param>
        /// <param name="account">[Dependency injection] Account service</param>
        public AccountController(ILogger<AuthController> logger, IAccount account)
        {
            _logger = logger;
            _account = account;
        }

        /// <summary>
        /// Create Client (Is 4 User)
        /// </summary>
        /// <param name="client">Client credentials</param>
        /// <returns>credentials</returns>
        [Route("Create")]
        [HttpPost]
        [Authorize(LocalApi.PolicyName)]
        public async Task<ActionResult> Create([FromBody] ClientCreation client)
        {
            try
            {
                await _account.Create(client);
                return Ok(client);
            }
            catch (HttpResquestResponseException ex)
            {
                _logger.LogDebug(ex.Message);
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (SecurityRuleException ex)
            {
                _logger.LogDebug(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return BadRequest("The operation could not be completed");
            }
        }


        /// <summary>
        /// Check a Client in IS4 (User)
        /// </summary>
        /// <param name="client">Client credentials</param>
        /// <returns>new credentials</returns>
        [Route("Check")]
        [HttpPost]
        [Authorize(LocalApi.PolicyName)]
        public async Task<ActionResult> Check([FromBody] ClientCreation client)
        {
            try
            {
                await _account.Check(client);
                return Ok(client);

            }
            catch (HttpResquestResponseException ex)
            {
                _logger.LogDebug(ex.Message);
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (SecurityRuleException ex)
            {
                _logger.LogDebug(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return BadRequest("The operation could not be completed");
            }
        }
    }
}
