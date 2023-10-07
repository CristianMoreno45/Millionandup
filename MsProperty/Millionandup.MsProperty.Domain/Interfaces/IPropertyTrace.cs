using Millionandup.MsProperty.Domain.AggregatesModel;

namespace Millionandup.MsProperty.Domain.Interfaces
{
    /// <summary>
    /// Property trace
    /// </summary>
    public interface IPropertyTrace
    {
        /// <summary>
        /// Trace identifier
        /// </summary>
        Guid PropertyTraceId { get; set; }

        /// <summary>
        /// Date of the sale
        /// </summary>
        DateTime DateSale { get; set; }

        /// <summary>
        /// Property name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Property value
        /// </summary>
        decimal Value { get; set; }

        /// <summary>
        /// Taxes value
        /// </summary>
        decimal Tax { get; set; }

        /// <summary>
        /// Property ID
        /// </summary>
        Guid PropertyId { get; set; }

        /// <summary>
        /// Property owner of this record
        /// </summary>
        Property Property { get; set; }
    }
}
