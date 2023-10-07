using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Millionandup.MsIdentityServer.AggregatesModel;

namespace Millionandup.MsIdentityServer.Infrastructure
{
    /// <summary>
    /// Context of the microservice
    /// </summary>
    public class IdentityDbContext : IdentityDbContext<AppUser>
    {
        /// <summary>EntityConfiguration
        /// Class constructor
        /// </summary>
        /// <param name="options">options</param>
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }
        /// <summary>
        /// Migration settings are applied
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Name = Constants.Role.CONSUMER, NormalizedName = Constants.Role.CONSUMER.ToUpper() });
        }
    }
}
