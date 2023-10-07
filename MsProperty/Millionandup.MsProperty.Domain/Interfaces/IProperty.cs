using Millionandup.Framework.DTO;
using Millionandup.MsProperty.Domain.AggregatesModel;
using Millionandup.MsProperty.Domain.DTO;

namespace Millionandup.MsProperty.Domain.Interfaces
{
    /// <summary>
    /// Property contract
    /// </summary>
    public interface IProperty
    {
        #region Properties
        /// <summary>
        /// Property ID
        /// </summary>
        Guid PropertyId { get; set; }

        /// <summary>
        /// Property name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Property Address
        /// </summary>
        string Address { get; set; }

        /// <summary>
        /// Property Price
        /// </summary>
        decimal Price { get; set; }

        /// <summary>
        /// Code Internal
        /// </summary>
        string CodeInternal { get; set; }

        /// <summary>
        /// Property Age in years
        /// </summary>
        int Year { get; set; }

        /// <summary>
        /// Event trace
        /// </summary>
        ICollection<PropertyTrace> PropertyTrace { get; set; }

        /// <summary>
        /// Images related to property
        /// </summary>
        ICollection<PropertyImage> PropertyImage { get; set; }

        #endregion

        #region Methods
        /// <summary>
        /// Allows you to create a property
        /// </summary>
        /// <param name="newProperty">New property</param>
        /// <returns>Created property</returns>
        Property CreatePropertyBuilding(CreatePropertyBuildingRequest newProperty);

        /// <summary>
        /// Allows you to add an image to a property
        /// </summary>
        /// <param name="image">Image to attach</param>
        /// <returns>Updated property</returns>
        /// 
        PropertyImage AddImageFromProperty(AddImageFromPropertyRequest image);

        /// <summary>
        /// Allows you to change the price of a property.
        /// </summary>
        /// <param name="newValue">New price</param>
        /// <returns>Updated property</returns>
        Property ChangePrice(ChangePriceRequest newValue);

        /// <summary>
        /// Allows you to change the data of a property.
        /// </summary>
        /// <param name="propertyDto">new data of property</param>
        /// <returns>Updated property</returns>
        Property UpdateProperty(UpdatePropertyRequest propertyDto);

        /// <summary>
        /// Allows extracting property information according to a filter
        /// </summary>
        /// <param name="propertyFilter">Property filter</param>
        /// <returns>List of properties</returns>
        PaggedResult<List<Property>> ListPropertyWithFilters(PropertyFilter propertyFilter);

        #endregion
    }

}
