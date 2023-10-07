using FluentValidation;
using Millionandup.MsProperty.Domain.AggregatesModel;
using Millionandup.MsProperty.Domain.DTO;
using Millionandup.MsProperty.Domain.Interfaces;
using MockQueryable.Moq;
using Moq;

namespace Millionandup.MsProperty.Test.Domain
{
    /// <summary>
    /// There are de base of Property Domain tests
    /// </summary>
    public abstract class PropertyTestsBase
    {
        protected List<Property> _propertyList;
        protected Guid _propertyId;
        protected OwnerTests _ownerTests;

        /// <summary>
        /// Create request and Property type models with the same information
        /// </summary>
        /// <param name="name">Property Name</param>
        /// <param name="address">Property Address</param>
        /// <param name="codeInternal">Property internal code</param>
        /// <param name="price">Property price</param>
        /// <param name="year">Property Age in years</param>
        /// <param name="propertyDto">(Out) Object <see cref="CreatePropertyBuildingRequest"/></param>
        /// <param name="newProperty">(Out) Object <see cref="Property"/></param>
        protected static void CreateModels(string name, string address, string codeInternal, decimal price, int year, out CreatePropertyBuildingRequest propertyDto, out Property newProperty)
        {
            propertyDto = new CreatePropertyBuildingRequest()
            {
                Name = name,
                Address = address,
                CodeInternal = codeInternal,
                Price = price,
                Year = year
            };
            newProperty = new Property()
            {
                Name = name,
                Address = address,
                CodeInternal = codeInternal,
                Price = price,
                Year = year
            };
        }

        /// <summary>
        /// These instructions will be executed before each test.
        /// </summary>
        [SetUp]
        protected void Setup()
        {
            _ownerTests = new OwnerTests();
            _propertyId = Guid.NewGuid();
            _propertyList = new List<Property>() {
                new Property() { PropertyId = _propertyId, Name="Property number one", Address="Avenue ever green, 123", CodeInternal="C0123456", Price=1000000,Year=5}
            };
        }



        /// <summary>
        /// Allows the creation of a repository based on mock templates
        /// </summary>
        /// <param name="name">Propiety name</param>
        /// <param name="code">Propiety code internal</param>
        /// <returns>Repository <see cref="IPropertyModel"/></returns>
        protected Mock<IPropertyModel> CreateMock(string name, string code)
        {
            IQueryable<Property>? mock = _propertyList.BuildMock();
            var modelMock = new Mock<IPropertyModel>();
            modelMock.Setup(b => b.GetAll()).Returns(mock);
            modelMock.Setup(x => x.GetByFilter(x => x.Name.ToUpper() == name.ToUpper()))
                .Returns(_propertyList.Where(x => x.Name.ToUpper() == name.ToUpper()).AsQueryable());
            modelMock.Setup(x => x.GetByFilter(x => x.CodeInternal.ToUpper() == code.ToUpper()))
                .Returns(_propertyList.Where(x => x.CodeInternal.ToUpper() == code.ToUpper()).AsQueryable());
            return modelMock;
        }
    }
}