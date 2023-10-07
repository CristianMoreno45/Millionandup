namespace Millionandup.MsProperty.Domain.DTO
{
    /// <summary>
    /// Data necessary to allow the updating of a property.
    /// </summary>
    public class UpdatePropertyRequest: CreatePropertyBuildingRequest
    {
        /// <summary>
        /// Property Id
        /// </summary>
        public Guid? PropertyId { get; set; }
    }
}
