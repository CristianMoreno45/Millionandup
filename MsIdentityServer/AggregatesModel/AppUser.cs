using Microsoft.AspNetCore.Identity;
namespace Millionandup.MsIdentityServer.AggregatesModel
{
    // Represent a session
    public class AppUser : IdentityUser
    {
        // Add additional profile data for application users by adding properties to this class
        public string IdUser { get; set; }
    }
}



