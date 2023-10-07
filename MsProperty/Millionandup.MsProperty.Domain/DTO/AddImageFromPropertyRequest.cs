namespace Millionandup.MsProperty.Domain.DTO
{
    /// <summary>
    /// Data required to allow adding an image to a property
    /// </summary>
    public class AddImageFromPropertyRequest
    {
        /// <summary>
        /// Property Id List
        /// </summary>
        public List<Guid>? PropertyIdList { get; set; }

        /// <summary>
        /// Resource's Url 
        /// </summary>
        public string? File { get; set; }

        /// <summary>
        /// Code Internal List
        /// </summary>
        public List<string>? CodeInternalList { get; set; }
    }
}
