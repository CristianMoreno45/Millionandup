using Millionandup.MsIdentityServer.DTO;

namespace Millionandup.MsIdentityServer.AggregatesModel
{
    /// <summary>
    /// Represent a Generic acount service
    /// </summary>
    public interface IAccount
    {
        /// <summary>
        /// Create Client (Is 4 User)
        /// </summary>
        /// <param name="client">Client credentials</param>
        /// <returns>task</returns>
        Task Create(ClientCreation client);

        /// <summary>
        /// Check Client into is4
        /// </summary>
        /// <param name="client">Client credentials</param>
        /// <returns></returns>
        /// <exception cref="HttpResquestResponseException"></exception>
        Task Check(ClientCreation client);
    }
}
