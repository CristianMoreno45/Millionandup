namespace Millionandup.MsIdentityServer.DTO
{
    /// <summary>
    /// Data model that represents a basic authorization fields
    /// </summary>
    public class ClientCreation
    {
        /// <summary>
        /// Client identifier (same user) in IS4
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Client secret in IS4
        /// </summary>
        public string? Secret { get; set; }
        /// <summary>
        /// Proof of authorization
        /// </summary>
        public string? RefreshSecret { get; set; }
        /// <summary>
        /// Expiration token (Epoch / unix time)
        /// </summary>
        public long ExpireSecret { get; set; }
        /// <summary>
        /// resource subject of the access request 
        /// </summary>
        public List<string>? Scopes { get; set; }
        /// <summary>
        /// User identification in external system
        /// </summary>
        public string? ExternalUserId { get; set; }

    }
}
