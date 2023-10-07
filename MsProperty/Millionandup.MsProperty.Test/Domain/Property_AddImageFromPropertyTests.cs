using FluentValidation;
using Millionandup.Framework.Domain.Exceptions;
using Millionandup.MsProperty.Domain.AggregatesModel;
using Millionandup.MsProperty.Domain.DTO;
using Millionandup.MsProperty.Domain.ModelValidation;
using System.Text;

namespace Millionandup.MsProperty.Test.Domain
{
    /// <summary>
    /// DDD tests - Property
    /// </summary>
    [TestFixture]
    public class Property_AddImageFromPropertyTests : PropertyTestsBase
    {

        
        #region AddImageFromProperty
        /// <summary>
        /// [Method]: AddImageFromProperty - [Expected result]:Success - [Case]: ByPropertyId
        /// </summary>
        [Test]
        public void SuccessResult_ByPropertyId()
        {
            //Arrange
            // > data
            string name = "Property number two";
            string codeInternal = "C0123456";
            Guid ownerId = Guid.NewGuid();
            string file = "https://www.helloword.com/foo.jpg";

            // > models
            AddImageFromPropertyRequest image = new() { PropertyIdList = new List<Guid> { _propertyId }, File = file };

            // > Mock
            var modelMock = CreateMock(name, codeInternal);
            modelMock.Setup(x => x.GetByFilter(x => image.PropertyIdList.Contains(x.PropertyId)))
               .Returns(_propertyList.Where(x => image.PropertyIdList.Contains(x.PropertyId)).AsQueryable());
            var mockObject = modelMock.Object;

            // > Dependency injection
            IValidator<Property> validator = new PropertyValidator(mockObject);
            IValidator<PropertyImage> propertyImageValidator = new PropertyImageValidator();
            Owner ownerService = _ownerTests.GetOwnerService(ownerId, null);
            Property propertyService = new(mockObject, ownerService, validator, propertyImageValidator);

            //Act
            propertyService.AddImageFromProperty(image);
            var property = mockObject.GetByFilter(x => image.PropertyIdList.Contains(x.PropertyId)).FirstOrDefault();

            //Assert
            Assert.That(property?.PropertyImage.FirstOrDefault()?.File ?? "", Is.EqualTo(file));
        }

        /// <summary>
        /// [Method]: AddImageFromProperty - [Expected result]:Success - [Case]: ByCodeInternal
        /// </summary>
        [Test]
        public void SuccessResult_ByCodeInternal()
        {
            //Arrange
            // > data
            string name = "Property number two";
            string codeInternal = "C0123456";
            Guid ownerId = Guid.NewGuid();
            string file = "https://www.helloword.com/foo.jpg";

            // > models
            AddImageFromPropertyRequest image = new() { CodeInternalList = new List<string> { codeInternal }, File = file };

            // > Mock
            var modelMock = CreateMock(name, codeInternal);
            modelMock.Setup(x => x.GetByFilter(x => image.PropertyIdList.Contains(x.PropertyId)))
               .Returns(_propertyList.Where(x => image.PropertyIdList.Contains(x.PropertyId)).AsQueryable());
            modelMock.Setup(x => x.GetByFilter(x => image.CodeInternalList.Contains(x.CodeInternal)))
               .Returns(_propertyList.Where(x => image.CodeInternalList.Contains(x.CodeInternal)).AsQueryable());
            var mockObject = modelMock.Object;

            // > Dependency injection
            IValidator<Property> validator = new PropertyValidator(mockObject);
            IValidator<PropertyImage> propertyImageValidator = new PropertyImageValidator();
            Owner ownerService = _ownerTests.GetOwnerService(ownerId, null);
            Property propertyService = new(mockObject, ownerService, validator, propertyImageValidator);

            //Act
            propertyService.AddImageFromProperty(image);
            var property = mockObject.GetByFilter(x => image.CodeInternalList.Contains(x.CodeInternal)).FirstOrDefault();

            //Assert
            Assert.That(property?.PropertyImage.FirstOrDefault()?.File ?? "", Is.EqualTo(file));
        }

        /// <summary>
        /// [Method]: AddImageFromProperty - [Expected result]:ThrowsException - [Case]: <see cref="PropertyImageValidator.MessagesError.FILE_IS_MANDATORY"/>
        /// </summary>
        [Test]
        public void ThrowsException_FileIsMandatory()
        {
            //Arrange
            // > data
            string name = "Property number two";
            string codeInternal = "C0123456";
            Guid ownerId = Guid.NewGuid();
            string file = "";

            //Arrange and act
            InvalidModelException ex = AddImageFromPropertyCommonArrangeAndAct(name, codeInternal, ownerId, file);

            //Assert
            Assert.That(ex.Message, Is.EqualTo(PropertyImageValidator.MessagesError.FILE_IS_MANDATORY));
        }

        /// <summary>
        /// [Method]: AddImageFromProperty - [Expected result]:ThrowsException - [Case]: <see cref="PropertyImageValidator.MessagesError.FILEURL_HAS_INCORRECT_FORMAT"/>
        /// </summary>
        [Test]
        public void ThrowsException_FileUrlHasIncorrectFormat()
        {
            //Arrange
            // > data
            string name = "Property number two";
            string codeInternal = "C0123456";
            Guid ownerId = Guid.NewGuid();
            string file = "www.fooo.com";

            //Arrange and act
            InvalidModelException ex = AddImageFromPropertyCommonArrangeAndAct(name, codeInternal, ownerId, file);

            //Assert
            Assert.That(ex.Message, Is.EqualTo(PropertyImageValidator.MessagesError.FILEURL_HAS_INCORRECT_FORMAT));
        }

        /// <summary>
        /// [Method]: AddImageFromProperty - [Expected result]:ThrowsException - [Case]: <see cref="PropertyImageValidator.MessagesError.PROPERTIES_DONT_EXIST"/>
        /// </summary>
        [Test]
        public void ThrowsException_PropertyDontExist()
        {
            //Arrange
            // > data
            string name = "Property number two";
            string codeInternal = "";
            Guid ownerId = Guid.NewGuid();
            string file = "https://www.helloword.com/foo.jpg";

            //Arrange and act
            InvalidModelException ex = AddImageFromPropertyCommonArrangeAndAct(name, codeInternal, ownerId, file, new AddImageFromPropertyRequest() { CodeInternalList = new List<string>(), File = file });

            //Assert
            Assert.That(ex.Message, Is.EqualTo(PropertyImageValidator.MessagesError.PROPERTIES_DONT_EXIST));
        }

        /// <summary>
        /// [Method]: AddImageFromProperty - [Expected result]:ThrowsException - [Case]: File - <see cref="PropertyImageValidator.MessagesError.MAX_LENGHT_STRING"/>
        /// </summary>
        [Test]
        public void ThrowsException_FileMaxLenght()
        {
            //Arrange
            // > data
            int maxLenght = 2048;
            string name = "Property number two";
            string codeInternal = "C0123456";
            Guid ownerId = Guid.NewGuid();
            string file = "https://www.helloword.com/foo.jpg";
            int baseLenght = file.Length;
            StringBuilder urlString = new StringBuilder();
            for (int i = 0; i < maxLenght - baseLenght; i++)
            {
                urlString.Append('o');
            }
            file = $"https://www.helloword.com/foo{urlString}.jpg";

            //Arrange and act
            InvalidModelException ex = AddImageFromPropertyCommonArrangeAndAct(name, codeInternal, ownerId, file);

            //Assert
            Assert.That(ex.Message, Is.EqualTo(string.Format(PropertyImageValidator.MessagesError.MAX_LENGHT_STRING, "File", maxLenght)));
        }

        #endregion
        #region Common Methods

        /// <summary>
        ///  Common group logic for multiple tests (AddImageFromProperty in ThrowsException only)
        /// </summary>
        /// <param name="name">Property Name</param>
        /// <param name="codeInternal">Property internal code</param>
        /// <param name="ownerId">Owner Id</param>
        /// <param name="file">Resource's Url</param>
        /// <param name="imageByRef">It is the request that will be sent (<see cref="AddImageFromPropertyRequest"/>), if it is not sent, it will fill whit _propertyId like a PropertyId</param>
        /// <returns>Returns the detected exception</returns>
        private InvalidModelException AddImageFromPropertyCommonArrangeAndAct(string name, string codeInternal, Guid ownerId, string file, AddImageFromPropertyRequest imageByRef = null)
        {
            AddImageFromPropertyRequest image;
            // > models
            if (imageByRef != null)
                image = imageByRef;
            else
                image = new AddImageFromPropertyRequest() { PropertyIdList = new List<Guid> { _propertyId }, File = file };

            // > Mock
            var modelMock = CreateMock(name, codeInternal);
            modelMock.Setup(x => x.GetByFilter(x => image.PropertyIdList.Contains(x.PropertyId)))
             .Returns(_propertyList.Where(x => image.PropertyIdList.Contains(x.PropertyId)).AsQueryable());
            modelMock.Setup(x => x.GetByFilter(x => image.CodeInternalList.Contains(x.CodeInternal)))
               .Returns(_propertyList.Where(x => image.CodeInternalList.Contains(x.CodeInternal)).AsQueryable());
            var mockObject = modelMock.Object;

            // > Dependency injection
            IValidator<Property> validator = new PropertyValidator(mockObject);
            IValidator<PropertyImage> propertyImageValidator = new PropertyImageValidator();
            Owner ownerService = _ownerTests.GetOwnerService(ownerId, null);
            Property propertyService = new(mockObject, ownerService, validator, propertyImageValidator);

            //Act 
            InvalidModelException ex = Assert.Throws<InvalidModelException>(delegate
            {
                propertyService.AddImageFromProperty(image);
            });
            return ex;
        }

        #endregion

    }
}