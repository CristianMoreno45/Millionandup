using Millionandup.Framework;
using Millionandup.MsProperty.Domain.AggregatesModel;

namespace Millionandup.MsProperty.Domain.Interfaces
{
    /// <summary>
    /// Interface that represents the repository (data access) of the Property entity
    /// </summary>
    public interface IPropertyModel : IRepository<Property>
    {
    }
}
