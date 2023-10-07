using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Millionandup.MsProperty.Domain.AggregatesModel;

namespace Millionandup.MsProperty.Infrastructure.Repository.EntityConfigurations
{
    /// <summary>
    /// Class with entity configurations
    /// </summary>
    public class OwnerEntityConfiguration : IEntityTypeConfiguration<Owner>
    {
        /// <summary>
        /// Configure entity relationships and properties
        /// </summary>
        /// <param name="entityConfiguration">Entity to configure</param>
        public void Configure(EntityTypeBuilder<Owner> entityConfiguration)
        {
            //The name of the table and the schema are defined
            entityConfiguration.ToTable("Owner", "dbo");
            entityConfiguration.HasIndex(a => a.OwnerId);
            entityConfiguration.Property(e => e.OwnerId)
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier")
                .HasComment("Owner ID");
            entityConfiguration.Property(e => e.Name).HasColumnType("nvarchar(80)").IsRequired().HasComment("Owner name");
            entityConfiguration.Property(e => e.Address).HasColumnType("nvarchar(125)").IsRequired().HasComment("Owner Address");
            entityConfiguration.Property(e => e.Photo).HasColumnType("nvarchar(2048)").IsRequired().HasComment("Owner Photo URL");
            entityConfiguration.Property(e => e.Birthday).HasColumnType("date").IsRequired().HasComment("Owner Birthday");
            entityConfiguration.HasMany(a => a.Property).WithMany(c => c.Owner).UsingEntity("OwnerByProperty");

        }
    }
}
