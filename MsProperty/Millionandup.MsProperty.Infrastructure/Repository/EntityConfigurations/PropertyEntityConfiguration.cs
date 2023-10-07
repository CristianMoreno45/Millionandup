using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Millionandup.MsProperty.Domain.AggregatesModel;

namespace Millionandup.MsProperty.Infrastructure.Repository.EntityConfigurations
{
    /// <summary>
    /// Class with entity configurations
    /// </summary>
    public class PropertyEntityConfiguration : IEntityTypeConfiguration<Property>
    {
        /// <summary>
        /// Configure entity relationships and properties
        /// </summary>
        /// <param name="entityConfiguration">Entity to configure</param>
        public void Configure(EntityTypeBuilder<Property> entityConfiguration)
        {
            //The name of the table and the schema are defined
            entityConfiguration.ToTable("Property", "dbo");
            entityConfiguration.HasIndex(a => a.PropertyId);
            entityConfiguration.Property(e => e.PropertyId)
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier")
                .HasComment("Property ID");
            entityConfiguration.Property(e => e.Name).HasColumnType("nvarchar(80)").IsRequired().HasComment("Property name");
            entityConfiguration.Property(e => e.Address).HasColumnType("nvarchar(125)").IsRequired().HasComment("Property Address");
            entityConfiguration.Property(e => e.Price).HasColumnType("money").IsRequired().HasComment("Property Price");
            entityConfiguration.Property(e => e.CodeInternal).HasColumnType("nvarchar(50)").IsRequired().HasComment("Code Internal");
            entityConfiguration.Property(e => e.Year).HasColumnType("int").IsRequired().HasComment("Property Age in years");

            entityConfiguration.HasMany(a => a.PropertyTrace).WithOne(c => c.Property).HasForeignKey(a => a.PropertyId);
            entityConfiguration.HasMany(a => a.PropertyImage).WithMany(c => c.Property).UsingEntity("PropertyImageByProperty");
            entityConfiguration.HasMany(a => a.Owner).WithMany(c => c.Property).UsingEntity("OwnerByProperty");

        }
    }
}
