using Millionandup.MsProperty.Domain.Interfaces;

namespace Millionandup.MsProperty.Domain.AggregatesModel
{
    /// <summary>
    /// Property image Business model
    /// </summary>
    public class PropertyImage: IPropertyImage
    {
        /// <summary>
        /// Property image ID
        /// </summary>
        public Guid PropertyImageId { get; set; }

        /// <summary>
        /// Resource's Url 
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// State of the image (true: active | false:inactive)
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Property owners
        /// </summary>
        public ICollection<Property> Property { get; set; }
    }
}
