
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Millionandup.Framework.Exceptions;

namespace Millionandup.MsIdentityServer.Controllers
{
    /// <summary>
    /// API Responsible for managing the validation of tokens in IS4
    /// </summary>
    [ApiController]
    [Route("Auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="configuration">[Dependency injection] App configuration</param>
        /// <param name="logger">[Dependency injection] logger</param>
        public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Get Token User, but this method uses a cache
        /// </summary>
        /// <param name="client">Client credentials</param>
        /// <returns>Token</returns>
        [HttpPost]
        [Route("Token")]
        public async Task<ActionResult> Token([FromForm] ClientCredentialsTokenRequest credentials)
        {
            try
            {
                if (Request.Headers.Authorization.Count > 0)
                {
                    var authToken = Request.Headers.Authorization.ElementAt(0);
                    if (authToken == null)
                        throw new SecurityRuleException("The request doesn't have authorization header");
                    // decoding authToken we get decode value in 'Username:Password' format  
                    var decodeauthToken = System.Text.Encoding.UTF8.GetString(
                        Convert.FromBase64String(authToken.Replace("Basic ", "")));

                    // spliting decodeauthToken using ':'   
                    string[]? arrUserNameandPassword = decodeauthToken.Split(':');
                    if (arrUserNameandPassword == null) 
                        throw new SecurityRuleException("The request dont has basic credentials"); 

                    // at 0th postion of array we get username and at 1st we get password  
                    credentials.ClientId = arrUserNameandPassword[0];
                    credentials.ClientSecret = arrUserNameandPassword[1];
                    // Search client session into cache 
                    var tokenGetter = SessionMemory.GetInstance(credentials.ClientId, credentials.ClientSecret, credentials.Scope);
                    // Set authority
                    credentials.Address = _configuration["NetConnections:identityServer:host"] + _configuration["NetConnections:identityServer:tokenApi"];
                    int.TryParse(_configuration["NetConnections:identityServer:AdditionalSecondsToRefreshToken"], out int seconds);
                    // Check, is the client valid and alive?; otherwise throw error
                    await tokenGetter.CheckExpirationToken(credentials, seconds);
                    // on success, set token
                    string token = tokenGetter.Token;
                    return Ok(new { access_token = token, expires_in = tokenGetter.ExpiresIn, token_type = "Bearer", scope = credentials.Scope });
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message + " " + ex.InnerException?.Message);
            }
        }
    }
}
