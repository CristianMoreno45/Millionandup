using Millionandup.MsProperty.Domain.AggregatesModel;

namespace Millionandup.MsProperty.Domain.Interfaces
{
    /// <summary>
    /// Property image
    /// </summary>
    public interface IPropertyImage
    {
        /// <summary>
        /// Property image ID
        /// </summary>
        Guid PropertyImageId { get; set; }

        /// <summary>
        /// Resource's Url 
        /// </summary>
        string File { get; set; }

        /// <summary>
        /// State of the image (true: active | false:inactive)
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Property owners
        /// </summary>
        ICollection<Property> Property { get; set; }
    }
}
