using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Millionandup.Framework.Domain;
using Millionandup.Framework.Domain.Exceptions;
using Millionandup.Framework.Domain.ModelValidation;
using Millionandup.Framework.Domain.ModelValidation.Enums;
using Millionandup.Framework.DTO;
using Millionandup.MsProperty.Domain.DTO;
using Millionandup.MsProperty.Domain.Interfaces;
using Millionandup.MsProperty.Domain.ModelValidation;
using static Millionandup.MsProperty.Domain.ModelValidation.PropertyValidator;

namespace Millionandup.MsProperty.Domain.AggregatesModel
{
    /// <summary>
    /// Real state Business Model
    /// </summary>
    public class Property : BaseDomain<Property, IPropertyModel>, IProperty
    {
        #region Properties
        /// <summary>
        /// Property ID
        /// </summary>
        public Guid PropertyId { get; set; }

        /// <summary>
        /// Property name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Property Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Property Price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Code Internal
        /// </summary>
        public string CodeInternal { get; set; }

        /// <summary>
        /// Property Age in years
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Event trace
        /// </summary>
        public ICollection<PropertyTrace> PropertyTrace { get; set; }

        /// <summary>
        /// Images related to property
        /// </summary>
        public ICollection<PropertyImage> PropertyImage { get; set; }

        /// <summary>
        /// Property owners
        /// </summary>
        public ICollection<Owner> Owner { get; set; }

        #endregion

        #region Dependencies
        /// <summary>
        /// Validator of Property Image
        /// </summary>
        private IValidator<PropertyImage> PropertyImageValidator { get; set; }

        /// <summary>
        /// Domain of Owner
        /// </summary>
        private IOwner OwnerService { get; set; }
        #endregion

        /// <summary>
        /// Class constructor, DDD lite a POCO
        /// </summary>
        public Property()
        {
            Name = "";
            Address = "";
            CodeInternal = "";
            PropertyTrace = new HashSet<PropertyTrace>();
            PropertyImage = new HashSet<PropertyImage>();
        }

        /// <summary>
        /// Class constructor, DDD lite a service (dependency injection)
        /// </summary>
        /// <param name="repository">Repository</param>
        /// <param name="validator">DDD Validator</param>
        public Property(
            IPropertyModel repository,
            IOwner ownerService,
            IValidator<Property> validator,
            IValidator<PropertyImage> propertyImageValidator) : this()
        {
            _repository = repository;
            _validator = validator;
            PropertyImageValidator = propertyImageValidator;
            OwnerService = ownerService;
        }

        #region Methods

        /// <summary>
        /// Allows you to create a property
        /// </summary>
        /// <param name="newProperty">New property</param>
        /// <returns>Created property</returns>
        /// <exception cref="InvalidModelException">If this breaks any business rules</exception>
        public Property CreatePropertyBuilding(CreatePropertyBuildingRequest newProperty)
        {
            // validations of input
            if (newProperty == null)
                throw new InvalidModelException("Empty request.");

            if (newProperty.Owners == null || newProperty.Owners?.Count == 0)
                throw new InvalidModelException(PropertyValidator.MessagesError.OWNERS_IS_MANDATORY);

            var owners = OwnerService.GetByFilter(x => newProperty.Owners.Contains(x.OwnerId));
            if (newProperty.Owners.Count != owners.Count)
                throw new InvalidModelException(PropertyValidator.MessagesError.OWNERS_NOT_EXIST);

            Property property = new()
            {
                Name = newProperty.Name ?? "",
                Address = newProperty.Address ?? "",
                Price = newProperty.Price,
                CodeInternal = newProperty.CodeInternal ?? "",
                Year = newProperty.Year,
                Owner = owners,
                PropertyTrace = new List<PropertyTrace>{
                    new PropertyTrace() {
                        DateSale = DateTime.UtcNow,
                        Name = newProperty.Name ?? "",
                        Value = newProperty.Price,
                        Tax = GetTaxValue(newProperty.Price)
                    }
                }
            };
            // validations of DDD
            _validator.ValidateAndThrow(property, ValidationType.CREATE.ToString());
            return _repository.Add(property);
        }

        /// <summary>
        /// Allows you to add an image to a property
        /// </summary>
        /// <param name="image">Image to attach</param>
        /// <returns>Updated Property Image</returns>
        /// <exception cref="InvalidModelException">If this breaks any business rules</exception>
        public PropertyImage AddImageFromProperty(AddImageFromPropertyRequest image)
        {
            // validations of input
            if (image == null)
                throw new InvalidModelException("Empty request.");

            List<Property>? properties = null;
            PropertyImage newImagen = new()
            {
                File = image?.File ?? "",
                Enabled = true
            };

            // Search by Property Id first
            if (image?.PropertyIdList != null && image.PropertyIdList.Count > 0)
            {
                properties = _repository.GetByFilter(x => image.PropertyIdList.Contains(x.PropertyId)).ToList();
                if (properties.Count != image.PropertyIdList.Count)
                    throw new InvalidModelException(ModelValidation.PropertyImageValidator.MessagesError.PROPERTYID_DONT_EXIST);
            }
            // in case of Property Id will be null, Search by Code Internal
            else if (image?.CodeInternalList != null && image.CodeInternalList.Count > 0)
            {
                properties = _repository.GetByFilter(x => image.CodeInternalList.Contains(x.CodeInternal)).ToList();
                if (properties.Count != image.CodeInternalList.Count)
                    throw new InvalidModelException(ModelValidation.PropertyImageValidator.MessagesError.CODEINTERNAL_DONT_EXIST);
            }

            // in case of Property will be null, throw exception
            if (properties == null)
                throw new InvalidModelException(ModelValidation.PropertyImageValidator.MessagesError.PROPERTIES_DONT_EXIST);

            properties.ForEach(p =>
            {
                if (p.PropertyImage == null)
                    p.PropertyImage = new List<PropertyImage>();
                p.PropertyImage.Add(newImagen);
            });

            // validations of DDD
            PropertyImageValidator.ValidateAndThrow(newImagen, ValidationType.CREATE.ToString());
            _repository.SaveChanges();

            return newImagen;

        }

        /// <summary>
        /// Allows you to change the price of a property.
        /// </summary>
        /// <param name="newValue">New price</param>
        /// <returns>Updated property</returns>
        /// <exception cref="InvalidModelException">If this breaks any business rules</exception>
        public Property ChangePrice(ChangePriceRequest newValue)
        {
            // validation od input
            if (newValue == null)
                throw new InvalidModelException("Empty request.");

            Property? property = null;

            // Search by Property Id first
            if (newValue?.PropertyId != Guid.Empty)
                property = _repository.GetByFilter(x => newValue.PropertyId == x.PropertyId).FirstOrDefault();
            // in case of Property Id will be null, Search by Code Internal
            else if (!string.IsNullOrEmpty(newValue?.CodeInternal))
                property = _repository.GetByFilter(x => newValue.CodeInternal == x.CodeInternal).FirstOrDefault();

            // in case of Property will be null, throw exception
            if (property == null)
                throw new InvalidModelException(ModelValidation.PropertyImageValidator.MessagesError.PROPERTY_DONT_EXIST);

            property.Price = newValue.NewPrice ?? 0;
            if (property.PropertyTrace == null)
                property.PropertyTrace = new List<PropertyTrace>();

            property.PropertyTrace.Add(new PropertyTrace()
            {
                DateSale = DateTime.UtcNow,
                Name = property.Name ?? "",
                Value = newValue.NewPrice ?? 0,
                Tax = GetTaxValue(newValue.NewPrice ?? 0)
            });

            // Validation of DDD
            _validator.ValidateAndThrow(property, PropertyValidationType.UPDATE_VALUE.ToString());
            _repository.SaveChanges();
            return property;
        }

        /// <summary>
        /// Allows you to change the data of a property.
        /// </summary>
        /// <param name="property">new data of property</param>
        /// <returns>Updated property</returns>
        /// <exception cref="InvalidModelException">If this breaks any business rules</exception>
        public Property UpdateProperty(UpdatePropertyRequest propertyDto)
        {
            // validation of input
            if (propertyDto == null)
                throw new InvalidModelException("Empty request.");

            Property? property = null;

            // Search by Property Id first
            if (propertyDto?.PropertyId != Guid.Empty)
                property = _repository.GetByFilter(x => propertyDto.PropertyId == x.PropertyId).Include(p => p.Owner).FirstOrDefault();
            // in case of Property Id will be null, Search by Code Internal
            else if (!string.IsNullOrEmpty(propertyDto?.CodeInternal))
                property = _repository.GetByFilter(x => propertyDto.CodeInternal == x.CodeInternal).Include(p => p.Owner).FirstOrDefault();

            // in case of Property will be null, throw exception
            if (property == null)
                throw new InvalidModelException(ModelValidation.PropertyImageValidator.MessagesError.PROPERTY_DONT_EXIST);

            //Validate Owners
            if (propertyDto?.Owners == null || propertyDto.Owners?.Count == 0)
                throw new InvalidModelException(PropertyValidator.MessagesError.OWNERS_IS_MANDATORY);

            var owners = OwnerService.GetByFilter(x => propertyDto.Owners.Contains(x.OwnerId));
            if (propertyDto?.Owners?.Count != owners.Count)
                throw new InvalidModelException(PropertyValidator.MessagesError.OWNERS_NOT_EXIST);

            // apply DB Transaction
            _repository.StartOwnTransactionWithinContext(() =>
            {
                // remove Old Owners
                List<Owner>? oldowners = property.Owner.ToList();
                oldowners.ForEach(o =>
                {
                    property.Owner.Remove(o);
                });
                _repository.SaveChanges();

                // replace old information
                property.Name = propertyDto?.Name ?? "";
                property.Address = propertyDto?.Address ?? "";
                property.Price = propertyDto?.Price ?? 0;
                property.Year = propertyDto?.Year ?? 0;
                property.Owner = owners;

                if (property.PropertyTrace == null)
                    property.PropertyTrace = new List<PropertyTrace>();

                property.PropertyTrace.Add(new PropertyTrace()
                {
                    DateSale = DateTime.UtcNow,
                    Name = propertyDto?.Name ?? "",
                    Value = propertyDto?.Price ?? 0,
                    Tax = propertyDto?.Price ?? 0 * (decimal)0.1,
                });

                // validation of DDD
                _validator.ValidateAndThrow(property, ValidationType.UPDATE.ToString());
                _repository.SaveChanges();
            });

            return property;
        }

        /// <summary>
        /// Allows extracting property information according to a filter
        /// </summary>
        /// <param name="propertyFilter">Property filter</param>
        /// <returns>List of properties</returns>
        /// <exception cref="InvalidModelException">If this breaks any business rules</exception>
        public PaggedResult<List<Property>> ListPropertyWithFilters(PropertyFilter propertyFilter)
        {
            PaggedResult<List<Property>> result = new();
            IQueryable<Property> preliminarResult = _repository.GetAll();

            // apply filters

            if (propertyFilter.PropertyId != null && propertyFilter.PropertyId != Guid.Empty)
                preliminarResult = preliminarResult.Where(p => p.PropertyId == propertyFilter.PropertyId);
            if (!string.IsNullOrEmpty(propertyFilter.Name))
                preliminarResult = preliminarResult.Where(p => p.Name.ToUpper().Contains(propertyFilter.Name.ToUpper()));
            if (!string.IsNullOrEmpty(propertyFilter.Address))
                preliminarResult = preliminarResult.Where(p => p.Address.ToUpper().Contains(propertyFilter.Address.ToUpper()));
            if (propertyFilter.Year > 0)
                preliminarResult = preliminarResult.Where(p => p.Year == propertyFilter.Year);
            if (!string.IsNullOrEmpty(propertyFilter.CodeInternal))
                preliminarResult = preliminarResult.Where(p => p.CodeInternal.ToUpper().Contains(propertyFilter.CodeInternal.ToUpper()));
            if (propertyFilter.OwnerId != null && propertyFilter.OwnerId != Guid.Empty)
                preliminarResult = preliminarResult.Where(p => p.Owner.Select(op => op.OwnerId).Contains((Guid)propertyFilter.OwnerId));
            if (propertyFilter?.MinValue > 0)
                preliminarResult = preliminarResult.Where(p => p.Price >= propertyFilter.MinValue);
            if (propertyFilter?.MaxValue > 0)
                preliminarResult = preliminarResult.Where(p => p.Price <= propertyFilter.MaxValue);

            // apply includes

            if (propertyFilter?.WithOwners ?? false)
                preliminarResult = preliminarResult.Include(p => p.Owner);
            if (propertyFilter?.WithPropertyImages ?? false)
                preliminarResult = preliminarResult.Include(p => p.PropertyImage);
            if (propertyFilter?.WithPropertyTraces ?? false)
                preliminarResult = preliminarResult.Include(p => p.PropertyTrace);

            // resolve paggination strategy
            int page = propertyFilter?.Page <= 0 ? 1 : propertyFilter?.Page ?? 0;
            int pagSize = propertyFilter?.PageSize <= 0 ? 10 : propertyFilter?.PageSize ?? 0;
            int skip = pagSize * (page - 1);
            int count = preliminarResult.Count();

            // apply paggination and response
            result.Result = preliminarResult.Skip(skip).Take(pagSize).ToList();
            result.FullResultCount = count;
            return result;
        }

        /// <summary>
        /// Extract tax from price
        /// </summary>
        /// <param name="price">Property price</param>
        /// <returns>tax</returns>
        private static decimal GetTaxValue(decimal price) => price * (decimal)0.12;
        #endregion
    }
}
