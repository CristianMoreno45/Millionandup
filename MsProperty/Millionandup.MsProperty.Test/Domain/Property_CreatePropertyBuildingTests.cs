using FluentValidation;
using Millionandup.Framework.Domain.Exceptions;
using Millionandup.MsProperty.Domain.AggregatesModel;
using Millionandup.MsProperty.Domain.DTO;
using Millionandup.MsProperty.Domain.Interfaces;
using Millionandup.MsProperty.Domain.ModelValidation;
using MockQueryable.Moq;
using Moq;

namespace Millionandup.MsProperty.Test.Domain
{
    /// <summary>
    /// DDD tests - Property
    /// </summary>
    [TestFixture]
    public partial class Property_CreatePropertyBuildingTests : PropertyTestsBase
    {

        #region CreatePropertyBuilding
        /// <summary>
        /// [Method]: CreatePropertyBuilding - [Expected result]:Success
        /// </summary>
        [Test]
        public void SuccessResult()
        {
            //Arrange
            // > data
            string name = "Property number two";
            string address = "Fake street, 123";
            string codeInternal = "C0789456G";
            decimal price = 2000000;
            int year = 2;
            Guid ownerId = Guid.NewGuid();
            // > models
            CreateModels(name, address, codeInternal, price, year, out CreatePropertyBuildingRequest propertyDto, out Property newProperty);
            propertyDto.Owners = new List<Guid> { ownerId };

            // > Mock
            var modelMock = CreateMock(name, codeInternal);
            var mockObject = modelMock.Object;

            // > Dependency injection
            IValidator<Property> validator = new PropertyValidator(mockObject);
            Owner ownerService = _ownerTests.GetOwnerService(ownerId, propertyDto);
            Property propertyService = new(mockObject, ownerService, validator, null);

            //Act
            propertyService.CreatePropertyBuilding(propertyDto);
            _propertyList.Add(newProperty);
            var created = mockObject.GetByFilter(x => x.Name.ToUpper() == name.ToUpper()).FirstOrDefault();

            //Assert
            Assert.That(created?.Name ?? "", Is.EqualTo(newProperty.Name));
        }

        /// <summary>
        /// [Method]: CreatePropertyBuilding - [Expected result]:ThrowsException - [Case]:<see cref="PropertyValidator.MessagesError.NAME_ALREADY_EXIST"/>
        /// </summary>
        [Test]
        public void ThrowsException_NameAlreadyExist()
        {
            //Arrange
            // > data
            string name = "Property number one";
            string address = "Fake street, 123";
            string codeInternal = "C0789456G";
            decimal price = 2000000;
            int year = 2;
            Guid ownerId = Guid.NewGuid();

            //Arrange and act
            InvalidModelException ex = CreatePropertyBuildingCommonArrangeAndAct(name, address, codeInternal, price, year, ownerId);

            //Assert
            Assert.That(ex.Message, Is.EqualTo(PropertyValidator.MessagesError.NAME_ALREADY_EXIST));
        }

        /// <summary>
        /// [Method]: CreatePropertyBuilding - [Expected result]:ThrowsException - [Case]:<see cref="PropertyValidator.MessagesError.NAME_INCORRECT_FORMAT"/>
        /// </summary>
        [Test]
        public void ThrowsException_NameHasIncorrectFormat()
        {
            //Arrange
            // > data
            string name = "Foo?";
            string address = "Fake street, 123";
            string codeInternal = "C0789456G";
            decimal price = 2000000;
            int year = 2;
            Guid ownerId = Guid.NewGuid();

            //Arrange and act
            InvalidModelException ex = CreatePropertyBuildingCommonArrangeAndAct(name, address, codeInternal, price, year, ownerId);

            //Assert
            Assert.That(ex.Message, Is.EqualTo(PropertyValidator.MessagesError.NAME_INCORRECT_FORMAT));
        }

        /// <summary>
        /// [Method]: CreatePropertyBuilding - [Expected result]:ThrowsException - [Case]:<see cref="PropertyValidator.MessagesError.NAME_IS_MANDATORY"/>
        /// </summary>
        [Test]
        public void ThrowsException_NameIsMandatory()
        {
            //Arrange
            // > data
            string name = "";
            string address = "Fake street, 123";
            string codeInternal = "C0789456G";
            decimal price = 2000000;
            int year = 2;
            Guid ownerId = Guid.NewGuid();

            //Arrange and act
            InvalidModelException ex = CreatePropertyBuildingCommonArrangeAndAct(name, address, codeInternal, price, year, ownerId);
            //Assert
            Assert.That(ex.Message, Is.EqualTo(PropertyValidator.MessagesError.NAME_IS_MANDATORY));
        }

        /// <summary>
        /// [Method]: CreatePropertyBuilding - [Expected result]:ThrowsException - [Case]:<see cref="PropertyValidator.MessagesError.PRICE_GREATER_THAN_ZERO"/>
        /// </summary>
        [Test]
        public void ThrowsException_PriceZero()
        {
            //Arrange
            // > data
            string name = "Property number two";
            string address = "Fake street, 123";
            string codeInternal = "C0789456G";
            decimal price = 0;
            int year = 2;
            Guid ownerId = Guid.NewGuid();

            //Arrange and act
            InvalidModelException ex = CreatePropertyBuildingCommonArrangeAndAct(name, address, codeInternal, price, year, ownerId);

            //Assert
            Assert.That(ex.Message, Is.EqualTo(PropertyValidator.MessagesError.PRICE_GREATER_THAN_ZERO));
        }

        /// <summary>
        /// [Method]: CreatePropertyBuilding - [Expected result]:ThrowsException - [Case]:<see cref="PropertyValidator.MessagesError.ADDRESS_IS_MANDATORY"/>
        /// </summary>
        [Test]
        public void ThrowsException_AddressIsMandatory()
        {
            //Arrange
            // > data
            string name = "Property number two";
            string address = "";
            string codeInternal = "C0789456G";
            decimal price = 2000000;
            int year = 2;
            Guid ownerId = Guid.NewGuid();

            //Arrange and act
            InvalidModelException ex = CreatePropertyBuildingCommonArrangeAndAct(name, address, codeInternal, price, year, ownerId);


            //Assert
            Assert.That(ex.Message, Is.EqualTo(PropertyValidator.MessagesError.ADDRESS_IS_MANDATORY));
        }

        /// <summary>
        /// [Method]: CreatePropertyBuilding - [Expected result]:ThrowsException - [Case]:<see cref="PropertyValidator.MessagesError.ADDRESS_INCORRECT_FORMAT"/>
        /// </summary>
        [Test]
        public void ThrowsException_AddressHasIncorrectFormat()
        {
            //Arrange
            // > data
            string name = "Property number two";
            string address = "Fake street, 123 ?";
            string codeInternal = "C0789456G";
            decimal price = 2000000;
            int year = 2;
            Guid ownerId = Guid.NewGuid();

            //Arrange and act
            InvalidModelException ex = CreatePropertyBuildingCommonArrangeAndAct(name, address, codeInternal, price, year, ownerId);

            //Assert
            Assert.That(ex.Message, Is.EqualTo(PropertyValidator.MessagesError.ADDRESS_INCORRECT_FORMAT));
        }

        /// <summary>
        /// Method]: CreatePropertyBuilding - [Expected result]:ThrowsException - [Case]:<see cref="PropertyValidator.MessagesError.CODE_IS_MANDATORY"/>
        /// </summary>
        [Test]
        public void ThrowsException_CodeIsMandatory()
        {            //Arrange
            // > data
            string name = "Property number two";
            string address = "Fake street, 123";
            string codeInternal = "";
            decimal price = 2000000;
            int year = 2;
            Guid ownerId = Guid.NewGuid();

            //Arrange and act
            InvalidModelException ex = CreatePropertyBuildingCommonArrangeAndAct(name, address, codeInternal, price, year, ownerId);

            //Assert
            Assert.That(ex.Message, Is.EqualTo(PropertyValidator.MessagesError.CODE_IS_MANDATORY));
        }

        /// <summary>
        /// [Method]: CreatePropertyBuilding - [Expected result]:ThrowsException - [Case]:<see cref="PropertyValidator.MessagesError.CODE_ALREADY_EXIST"/>
        /// </summary>
        [Test]
        public void ThrowsException_CodeAlreadyExist()
        {
            //Arrange
            // > data
            string name = "Property number two";
            string address = "Fake street, 123";
            string codeInternal = "C0123456";
            decimal price = 2000000;
            int year = 2;
            Guid ownerId = Guid.NewGuid();

            //Arrange and act
            InvalidModelException ex = CreatePropertyBuildingCommonArrangeAndAct(name, address, codeInternal, price, year, ownerId);

            //Assert
            Assert.That(ex.Message, Is.EqualTo(PropertyValidator.MessagesError.CODE_ALREADY_EXIST));
        }

        /// <summary>
        /// [Method]: CreatePropertyBuilding - [Expected result]:ThrowsException - [Case]:<see cref="PropertyValidator.MessagesError.OWNERS_IS_MANDATORY"/>
        /// </summary>
        [Test]
        public void ThrowsException_OwnerIsMandatory()
        {
            //Arrange
            // > data
            string name = "Property number two";
            string address = "Fake street, 123";
            string codeInternal = "C0789456G";
            decimal price = 2000000;
            int year = 2;
            Guid ownerId = Guid.NewGuid();

            //Arrange and act
            InvalidModelException ex = CreatePropertyBuildingCommonArrangeAndAct(name, address, codeInternal, price, year, ownerId, new List<Guid> { });

            //Assert
            Assert.That(ex.Message, Is.EqualTo(PropertyValidator.MessagesError.OWNERS_IS_MANDATORY));
        }

        /// <summary>
        /// [Method]: CreatePropertyBuilding - [Expected result]:ThrowsException - [Case]:<see cref="PropertyValidator.MessagesError.OWNERS_NOT_EXIST"/>
        /// </summary>
        [Test]
        public void ThrowsException_OwnerNotExist()
        {
            //Arrange
            // > data
            string name = "Property number two";
            string address = "Fake street, 123";
            string codeInternal = "C0789456G";
            decimal price = 2000000;
            int year = 2;
            Guid ownerId = Guid.NewGuid();

            //Arrange and act
            InvalidModelException ex = CreatePropertyBuildingCommonArrangeAndAct(name, address, codeInternal, price, year, ownerId, new List<Guid> { Guid.NewGuid() });

            //Assert
            Assert.That(ex.Message, Is.EqualTo(PropertyValidator.MessagesError.OWNERS_NOT_EXIST));
        }

        /// <summary>
        /// [Method]: CreatePropertyBuilding - [Expected result]:ThrowsException - [Case]: Name - <see cref="PropertyValidator.MessagesError.MAX_LENGHT_STRING"/>
        /// </summary>
        [Test]
        public void ThrowsException_NameMaxLenght()
        {
            //Arrange
            // > data
            string name = "123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 1";
            string address = "Fake street, 123";
            string codeInternal = "C0789456G";
            decimal price = 2000000;
            int year = 2;
            Guid ownerId = Guid.NewGuid();

            //Arrange and act
            InvalidModelException ex = CreatePropertyBuildingCommonArrangeAndAct(name, address, codeInternal, price, year, ownerId);

            //Assert
            Assert.That(ex.Message, Is.EqualTo(string.Format(PropertyValidator.MessagesError.MAX_LENGHT_STRING,"Name", 80)));
        }


        /// <summary>
        /// [Method]: CreatePropertyBuilding - [Expected result]:ThrowsException - [Case]: Address - <see cref="PropertyValidator.MessagesError.MAX_LENGHT_STRING"/>
        /// </summary>
        [Test]
        public void ThrowsException_AddressMaxLenght()
        {
            //Arrange
            // > data
            string name = "Property number two";
            string address = "123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456";
            string codeInternal = "C0789456G";
            decimal price = 2000000;
            int year = 2;
            Guid ownerId = Guid.NewGuid();

            //Arrange and act
            InvalidModelException ex = CreatePropertyBuildingCommonArrangeAndAct(name, address, codeInternal, price, year, ownerId);

            //Assert
            Assert.That(ex.Message, Is.EqualTo(string.Format(PropertyValidator.MessagesError.MAX_LENGHT_STRING, "Address", 125)));
        }

        /// <summary>
        /// [Method]: CreatePropertyBuilding - [Expected result]:ThrowsException - [Case]: Price - <see cref="PropertyValidator.MessagesError.MAX_LENGHT_MONEY"/>
        /// </summary>
        [Test]
        public void ThrowsException_PriceMaxValue()
        {
            //Arrange
            // > data
            string name = "Property number two";
            string address = "Fake street, 123";
            string codeInternal = "C0789456G";
            decimal price = 1000000000001;
            int year = 2;
            Guid ownerId = Guid.NewGuid();

            //Arrange and act
            InvalidModelException ex = CreatePropertyBuildingCommonArrangeAndAct(name, address, codeInternal, price, year, ownerId);

            //Assert
            Assert.That(ex.Message, Is.EqualTo(string.Format(PropertyValidator.MessagesError.MAX_LENGHT_MONEY, "Price", 1000000000000)));
        }

        /// <summary>
        /// [Method]: CreatePropertyBuilding - [Expected result]:ThrowsException - [Case]: CodeInternal - <see cref="PropertyValidator.MessagesError.MAX_LENGHT_STRING"/>
        /// </summary>
        [Test]
        public void ThrowsException_CodeInternalMaxLenght()
        {
            //Arrange
            // > data
            string name = "Property number two";
            string address = "Fake street, 123";
            string codeInternal = "123456789 123456789 123456789 123456789 123456789 1";
            decimal price = 2000000;
            int year = 2;
            Guid ownerId = Guid.NewGuid();

            //Arrange and act
            InvalidModelException ex = CreatePropertyBuildingCommonArrangeAndAct(name, address, codeInternal, price, year, ownerId);

            //Assert
            Assert.That(ex.Message, Is.EqualTo(string.Format(PropertyValidator.MessagesError.MAX_LENGHT_STRING, "CodeInternal", 50)));
        }

        #endregion

        #region Common Methods

        /// <summary>
        /// Common group logic for multiple tests (CreatePropertyBuilding in ThrowsException only)
        /// </summary>
        /// <param name="name">Property Name</param>
        /// <param name="address">Property Address</param>
        /// <param name="codeInternal">Property internal code</param>
        /// <param name="price">Property price</param>
        /// <param name="year">Property Age in years</param>
        /// <param name="ownerId">Owner Id</param>
        /// <param name="ownersRequest">It is the list of owners that will be sent in the request (<see cref="CreatePropertyBuildingRequest"/>), if it is not sent, it will take ownerId</param>
        /// <returns>Returns the detected exception</returns>
        private InvalidModelException CreatePropertyBuildingCommonArrangeAndAct(string name, string address, string codeInternal, decimal price, int year, Guid ownerId, List<Guid> ownersRequest = null)
        {
            // > Mock
            var modelMock = CreateMock(name, codeInternal);
            var mockObject = modelMock.Object;
            // > models
            CreateModels(name, address, codeInternal, price, year, out CreatePropertyBuildingRequest propertyDto, out Property newProperty);
            propertyDto.Owners = ownersRequest ?? new List<Guid> { ownerId };

            // > Dependency injection
            IValidator<Property> validator = new PropertyValidator(mockObject);
            Owner ownerService = _ownerTests.GetOwnerService(ownerId, propertyDto);
            Property propertyService = new(mockObject, ownerService, validator, null);

            //Act 
            InvalidModelException ex = Assert.Throws<InvalidModelException>(delegate
            {
                propertyService.CreatePropertyBuilding(propertyDto);
            });
            return ex;
        }
        #endregion

    }
}