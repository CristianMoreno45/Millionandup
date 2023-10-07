using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Millionandup.MsProperty.Domain.AggregatesModel;

namespace Millionandup.MsProperty.Infrastructure.Repository.EntityConfigurations
{
    /// <summary>
    /// Class with entity configurations
    /// </summary>
    public class PropertyImageEntityConfiguration : IEntityTypeConfiguration<PropertyImage>
    {
        /// <summary>
        /// Configure entity relationships and properties
        /// </summary>
        /// <param name="entityConfiguration">Entity to configure</param>
        public void Configure(EntityTypeBuilder<PropertyImage> entityConfiguration)
        {
            //The name of the table and the schema are defined
            entityConfiguration.ToTable("PropertyImage", "dbo");
            entityConfiguration.HasIndex(a => a.PropertyImageId);
            entityConfiguration.Property(e => e.PropertyImageId)
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier")
                .HasComment("Property image ID");
            entityConfiguration.Property(e => e.File).HasColumnType("nvarchar(2048)").IsRequired().HasComment("Date of the sale");
            entityConfiguration.Property(e => e.Enabled).HasColumnType("bit").IsRequired().HasComment("Log is eneable");
            entityConfiguration.HasMany(a => a.Property).WithMany(c => c.PropertyImage).UsingEntity("PropertyImageByProperty");

        }
    }
}
