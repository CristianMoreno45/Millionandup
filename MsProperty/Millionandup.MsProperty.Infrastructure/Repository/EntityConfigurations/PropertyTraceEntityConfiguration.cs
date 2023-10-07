using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Millionandup.MsProperty.Domain.AggregatesModel;

namespace Millionandup.MsProperty.Infrastructure.Repository.EntityConfigurations
{
    /// <summary>
    /// Class with entity configurations
    /// </summary>
    public class PropertyTraceEntityConfiguration : IEntityTypeConfiguration<PropertyTrace>
    {
        /// <summary>
        /// Configure entity relationships and properties
        /// </summary>
        /// <param name="entityConfiguration">Entity to configure</param>
        public void Configure(EntityTypeBuilder<PropertyTrace> entityConfiguration)
        {
            //The name of the table and the schema are defined
            entityConfiguration.ToTable("PropertyTrace", "dbo");
            entityConfiguration.HasIndex(a => a.PropertyTraceId);
            entityConfiguration.HasIndex(a => a.DateSale).IsDescending();
            entityConfiguration.Property(e => e.PropertyTraceId)
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier")
                .HasComment("Trace identifier");
            entityConfiguration.Property(e => e.DateSale).HasColumnType("datetime").IsRequired().HasComment("Date of the sale");
            entityConfiguration.Property(e => e.Name).HasColumnType("nvarchar(80)").IsRequired().HasComment("Property name");
            entityConfiguration.Property(e => e.Value).HasColumnType("money").IsRequired().HasComment("Property value");
            entityConfiguration.Property(e => e.Tax).HasColumnType("money").IsRequired().HasComment("Taxes value");
            entityConfiguration.Property(e => e.PropertyId).HasColumnType("uniqueidentifier").IsRequired().HasComment("Property ID");

            entityConfiguration.HasOne(a => a.Property).WithMany(c => c.PropertyTrace).HasForeignKey(a => a.PropertyId);

        }
    }
}
