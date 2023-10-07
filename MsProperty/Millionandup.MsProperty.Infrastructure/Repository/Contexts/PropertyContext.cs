using Microsoft.EntityFrameworkCore;
using Millionandup.MsProperty.Domain.AggregatesModel;
using Millionandup.MsProperty.Infrastructure.Repository.EntityConfigurations;

namespace Millionandup.MsProperty.Infrastructure.Repository.Contexts
{
    /// <summary>
    /// Context of the microservice
    /// </summary>
    public class PropertyContext : DbContext
    {
        /// <summary>
        /// Default Scheme
        /// </summary>
        private static readonly string DEFAULT_SCHEMA = "dbo";

        /// <summary>
        /// Property that represents the Property entity
        /// </summary>
        public virtual DbSet<Property> Property { get; set; }

        /// <summary>
        /// Property that represents the owner entity
        /// </summary>
        public virtual DbSet<Owner> Owner { get; set; }


        /// <summary>EntityConfiguration
        /// Class constructor
        /// </summary>
        /// <param name="options">options</param>
        public PropertyContext(DbContextOptions<PropertyContext> options) : base(options) { }

        /// <summary>
        /// Migration settings are applied
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DEFAULT_SCHEMA);
            // DB Behavior setting
            modelBuilder.ApplyConfiguration(new PropertyEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PropertyTraceEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PropertyImageEntityConfiguration());
            modelBuilder.ApplyConfiguration(new OwnerEntityConfiguration());

        }
    }
}
