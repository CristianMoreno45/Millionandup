namespace Millionandup.MsProperty.Domain.DTO
{
    /// <summary>
    /// Data necessary to allow changing the price of a property
    /// </summary>
    public class ChangePriceRequest
    {
        /// <summary>
        /// Property Id
        /// </summary>
        public Guid? PropertyId { get; set; }

        /// <summary>
        /// New property price
        /// </summary>
        public decimal? NewPrice { get; set; }

        /// <summary>
        /// Code Internal
        /// </summary>
        public string? CodeInternal { get; set; }
    }
}
