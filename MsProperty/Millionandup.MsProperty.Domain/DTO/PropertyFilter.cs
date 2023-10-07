namespace Millionandup.MsProperty.Domain.DTO
{
    /// <summary>
    /// Filters for the Property business object
    /// </summary>
    public class PropertyFilter : Filter
    {
        /// <summary>
        /// Property Id
        /// </summary>
        public Guid? PropertyId { get; set; }

        /// <summary>
        /// Property name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Property Address
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Code Internal
        /// </summary>
        public string? CodeInternal { get; set; }

        /// <summary>
        /// Property Age in years
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Filter per value [lower rank]: Default is 0
        /// </summary>
        public decimal MinValue { get; set; }

        /// <summary>
        /// Filter per value [top rank] : Default is 999999999
        /// </summary>
        public decimal MaxValue { get; set; }

        /// <summary>
        /// Owner Id
        /// </summary>
        public Guid? OwnerId { get; set; }

        /// <summary>
        /// Determines if the query should bring the list of owners: Default is false
        /// </summary>
        public bool WithOwners { get; set; }

        /// <summary>
        /// Determines if the query should bring the list of images: Default is false
        /// </summary>
        public bool WithPropertyImages { get; set; }

        /// <summary>
        /// Determines if the query should bring the list of events: Default is false
        /// </summary>
        public bool WithPropertyTraces { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public PropertyFilter()
        {
            MinValue = 0;
            MaxValue = 999999999;
            WithOwners = false;
            WithPropertyImages = false;
            WithPropertyTraces = false;
        }
    }
}
