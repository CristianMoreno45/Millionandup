namespace Millionandup.MsProperty.Domain.DTO
{
    /// <summary>
    /// Data necessary to allow the creation of a property.
    /// </summary>
    public class CreatePropertyBuildingRequest
    {
        /// <summary>
        /// Property name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Property Address
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Property Price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Code Internal
        /// </summary>
        public string? CodeInternal { get; set; }

        /// <summary>
        /// Property Age in years
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Property Owners
        /// </summary>
        public List<Guid>? Owners { get; set; }
    }
}
