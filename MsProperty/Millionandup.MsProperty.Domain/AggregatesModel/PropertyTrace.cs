using Millionandup.MsProperty.Domain.Interfaces;

namespace Millionandup.MsProperty.Domain.AggregatesModel
{
    /// <summary>
    /// Property trace business model
    /// </summary>
    public class PropertyTrace: IPropertyTrace
    {
        /// <summary>
        /// Trace identifier
        /// </summary>
        public Guid PropertyTraceId { get; set; }

        /// <summary>
        /// Date of the sale
        /// </summary>
        public DateTime DateSale { get; set; }

        /// <summary>
        /// Property name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Property value
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Taxes value
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// Property ID
        /// </summary>
        public Guid PropertyId { get; set; }

        /// <summary>
        /// Property owner of this record
        /// </summary>
        public Property Property { get; set; }

    }
}
