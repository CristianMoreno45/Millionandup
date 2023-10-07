using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Millionandup.Framework.Exceptions;
using Millionandup.Framework.Extensions;
using Millionandup.MsIdentityServer.DTO;
using Millionandup.MsIdentityServer.Services;
using IS4Entitites = IdentityServer4.EntityFramework.Entities;
namespace Millionandup.MsIdentityServer.AggregatesModel
{
    /// <summary>
    /// Class responsible for creating accounts in IS4
    /// </summary>
    public class Account : IAccount
    {
        private readonly ConfigurationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _is4Host;
        private readonly int _secretExpiration;
        private static string INTERNAL_KEY = "eNunB05Q3D3L4Ch1N4LaCHiN1T4s3P3rD10";

        public long ExpireSecret { get; set; }

        public Account(ConfigurationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _is4Host = _configuration["NetConnections:identityServer:host"];
            int.TryParse(_configuration["NetConnections:identityServer:secretExpiration"] ?? "3600", out _secretExpiration);
        }

        #region Operations
        /// <summary>
        /// Create Client (Is 4 User)
        /// </summary>
        /// <param name="client">Client credentials</param>
        /// <returns>task</returns>
        public async Task Create(ClientCreation client)
        {
            CreationRulesValidation(client);
            // if exist, delete client
            IS4Entitites.Client? cli = _context.Clients.FirstOrDefault(x => x.ClientId == client.ClientId);
            if (cli != null)
            {
                _context.Remove(cli);
                await _context.SaveChangesAsync();
            }
            // create a secret
            client.Secret = Guid.NewGuid().ToString();
            // create a client dto
            var clientModel = new Client()
            {
                ClientId = client.ClientId,
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = {
                        new Secret(client.Secret.Sha256())
                    },
                AllowedScopes = client.Scopes
            };
            // create client into db
            _context.Clients.Add(clientModel.ToEntity());
            await _context.SaveChangesAsync();
            //
            // update response post db action
            CalculateRefreshSecret(client);

        }

        /// <summary>
        /// Check Client into is4
        /// </summary>
        /// <param name="client">Client credentials</param>
        /// <returns></returns>
        /// <exception cref="HttpResquestResponseException"></exception>
        public async Task Check(ClientCreation client)
        {
            // Check Client 
            CheckRulesValidation(client);
            // Is Correct?, then update sercret, else throw Exception
            if (IsCorrectSigned(client))
            {
                if (client.ExpireSecret.GetDateTimeFromUnix() < DateTime.UtcNow)
                    throw new HttpResquestResponseException(401);
                else
                    CalculateRefreshSecret(client);
            }
            else
                throw new HttpResquestResponseException(400, "The RefreshSecret has been corrupted.");

        }

        #endregion

        #region Validators

        /// <summary>
        /// Check business rules to create user
        /// </summary>
        /// <param name="client">Client credentials</param>
        /// <exception cref="SecurityRuleException"></exception>
        public void CreationRulesValidation(ClientCreation client)
        {
            if (client.ClientId == null || client.ClientId == "")
                throw new SecurityRuleException("The ClientId is empty.");

            if (client.Scopes == null || client.Scopes.Count == 0)
                throw new SecurityRuleException("The Scopes is empty.");

            if (!_context.ApiScopes.Any(x => client.Scopes.Contains(x.Name)))
                throw new SecurityRuleException("The Scope is invalid.");
        }
        /// <summary>
        /// Check business rules to check user
        /// </summary>
        /// <param name="client">Client credentials</param>
        /// <exception cref="SecurityRuleException"></exception>
        public void CheckRulesValidation(ClientCreation client)
        {
            if (client.ClientId == null || client.ClientId == "")
                throw new SecurityRuleException("The ClientId is empty.");
            if (client.Secret == null || client.Secret == "")
                throw new SecurityRuleException("The ClientSecret is empty.");
            if (client.ExpireSecret == 0)
                throw new SecurityRuleException("The ExpireSecret is empty.");
            if (client.RefreshSecret == null || client.RefreshSecret == "")
                throw new SecurityRuleException("The RefreshSecret is empty.");
        }

        #endregion

        #region Util
        /// <summary>
        /// Check if a chant is trustworthy or not
        /// </summary>
        /// <param name="client">Client credentials</param>
        /// <returns></returns>
        private bool IsCorrectSigned(ClientCreation client)
        {
            using AesGcmService aesObj = new AesGcmService(INTERNAL_KEY);
            string key = GetKey(client);
            string requestKey = aesObj.Decrypt(client.RefreshSecret);
            return key == requestKey;
        }

        /// <summary>
        /// Calculate refresh secret
        /// </summary>
        /// <param name="client">Client credentials</param>
        private void CalculateRefreshSecret(ClientCreation client)
        {
            client.ExpireSecret = DateTime.UtcNow.AddSeconds(_secretExpiration).ToUnixTime();
            var aesObj = new AesGcmService(INTERNAL_KEY);
            string key = GetKey(client);
            client.RefreshSecret = aesObj.Encrypt(key);
        }

        /// <summary>
        /// Get a key in a standard format
        /// </summary>
        /// <param name="client">Client credentials</param>
        /// <returns></returns>
        private string GetKey(ClientCreation client)
        {
            return string.Format("{0}:{1}:{2}:{3}:{4}", client.Secret, client.ExpireSecret, _is4Host, INTERNAL_KEY, client.ExternalUserId);
        }
        #endregion

    }

   
}
