using IdentityModel.Client;

namespace Millionandup.MsIdentityServer
{
    /// <summary>
    /// Class in charge of managing the sessions
    /// </summary>
    internal class SessionMemory
    {
        /// <summary>
        /// List Session in memory
        /// </summary>
        private static readonly List<SessionMemory> _instance = new List<SessionMemory>();

        private const int DEFAULT_ADDITIONAL_SECONDS_TO_REFRESH = 3600;

        #region Properties
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
        private DateTime TokenExpirationDate { get; set; }
        private string _clientId { get; set; }
        private string _clientSecret { get; set; }
        private string _scope { get; set; }
        #endregion

        public SessionMemory(string clientId, string secret, string scope)
        {
            _clientId = clientId;
            _clientSecret = secret;
            _scope = scope;
        }
        #region Getters

        /// <summary>
        /// Get a Session by client, secret and scope
        /// </summary>
        /// <param name="clientId">Client Identificator</param>
        /// <param name="secret">Client secret</param>
        /// <param name="scope">Resource scope</param>
        /// <returns>Session</returns>
        public static SessionMemory GetInstance(string clientId, string secret, string scope) {
            var session = _instance.FirstOrDefault(x => x._clientId == clientId && x._clientSecret == secret && x._scope == scope);
            if (session == null) { 
                var newSession = new SessionMemory(clientId, secret, scope);
                _instance.Add(newSession);
                return newSession;
            }
            else
                return session;
        }
        /// <summary>
        /// Get current session token
        /// </summary>
        /// <returns>Bearer token</returns>
        public string GetTokenValue() => Token;

        /// <summary>
        /// Get current session expiration date
        /// </summary>
        /// <returns>Date and time in Epoch (unix) format, session end time</returns>
        public DateTime GetExpirationDate() => TokenExpirationDate;

        #endregion

        /// <summary>
        /// Verify that the client is valid and alive.
        /// </summary>
        /// <param name="tokenRequest">Token</param>
        /// <param name="additionalSecondsToRefreshToken">Additional seconds</param>
        /// <returns>Task</returns>
        public async Task CheckExpirationToken(ClientCredentialsTokenRequest tokenRequest,int additionalSecondsToRefreshToken = 0)
        {
            if (DateTime.Now >= TokenExpirationDate)
            {
                await GetToken(tokenRequest, additionalSecondsToRefreshToken);
            }
        }

        /// <summary>
        /// Valide el token, si es correcto, configure la información del token en la sesión actual; de lo contrario, lanzar error.
        /// </summary>
        /// <param name="tokenRequest">Token</param>
        /// <param name="additionalSecondsToRefreshToken">Additional seconds</param>
        /// <returns>Task</returns>
        /// <exception cref="Exception"></exception>
        public async Task GetToken(ClientCredentialsTokenRequest tokenRequest, int additionalSecondsToRefreshToken)
        {
            var client = new HttpClient();
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(tokenRequest);
            if (tokenResponse.IsError)
                throw new Exception("Error obtaining the access token to the service api, check the request");

            else
            {
                Token = tokenResponse.AccessToken;
                ExpiresIn = tokenResponse.ExpiresIn;
                TokenExpirationDate = DateTime.Now.AddSeconds(additionalSecondsToRefreshToken == 0 ? DEFAULT_ADDITIONAL_SECONDS_TO_REFRESH : additionalSecondsToRefreshToken);
            }
        }

        /// <summary>
        /// Create a <see cref="ClientCredentialsTokenRequest"/> object
        /// </summary>
        /// <param name="address">Authority address (IS4 Endpoint validation)</param>
        /// <param name="clientId">Client identification</param>
        /// <param name="secret">Client secret</param>
        /// <param name="scope">Resource scope</param>
        /// <returns>A client credentials token request object </returns>
        public static ClientCredentialsTokenRequest GetBasicCredential(string address, string clientId, string secret, string scope)
        {
            return new ClientCredentialsTokenRequest
            {
                Address = address,
                ClientId = clientId,
                ClientSecret = secret,
                Scope = scope
            };
        }
    }
}
