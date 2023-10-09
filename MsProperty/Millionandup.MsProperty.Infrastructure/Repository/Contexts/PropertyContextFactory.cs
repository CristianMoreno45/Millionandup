using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Millionandup.MsProperty.Infrastructure.Repository.Contexts
{
    // Crete a factory of de current context
    public class PropertyContextFactory : IDesignTimeDbContextFactory<PropertyContext>
    {
        /// <summary>
        /// Creating a new context to migrations
        /// </summary>
        /// <param name="args">args</param>
        /// <returns>contect <see cref="PropertyContext"/></returns>
        public PropertyContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PropertyContext>();
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=millionandup_msproperty;Integrated Security=SSPI;TrustServerCertificate=True;");

            return new PropertyContext(optionsBuilder.Options);
        }
    }
}
