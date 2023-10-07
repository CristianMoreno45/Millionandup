using Millionandup.MsProperty.Domain.AggregatesModel;
using Millionandup.MsProperty.Domain.DTO;
using Millionandup.MsProperty.Domain.Interfaces;
using MockQueryable.Moq;
using Moq;

namespace Millionandup.MsProperty.Test.Domain
{
    /// <summary>
    /// DDD tests - Owner
    /// </summary>
    [TestFixture]
    public class OwnerTests
    {
        /// <summary>
        /// Test Data
        /// </summary>
        private List<Owner> _ownerList;

        /// <summary>
        /// Owner Id
        /// </summary>
        private Guid _ownerId;

        /// <summary>
        /// Owner Name
        /// </summary>
        private string _name;

        /// <summary>
        /// These instructions will be executed before each test.
        /// </summary>
        [SetUp]
        public void Setup()
        {

        }

        /// <summary>
        /// Allows the creation of a repository based on mock templates
        /// </summary>
        /// <param name="name">Name of Owner</param>
        /// <param name="ownerId">Owner Id</param>
        /// <param name="newProperty">Property request to create</param>
        /// <returns>Repository <see cref="IOwnerModel"/></returns>
        private Mock<IOwnerModel> CreateMock(string name, Guid ownerId, CreatePropertyBuildingRequest newProperty)
        {
            IQueryable<Owner>? mock = _ownerList.BuildMock();
            var modelMock = new Mock<IOwnerModel>();

            modelMock.Setup(b => b.GetAll()).Returns(mock);
            modelMock.Setup(x => x.GetByFilter(x => x.Name.ToUpper() == name.ToUpper()))
                .Returns(_ownerList.Where(x => x.Name.ToUpper() == name.ToUpper()).AsQueryable());
            modelMock.Setup(x => x.GetByFilter(x => x.OwnerId.Equals(ownerId)))
              .Returns(_ownerList.Where(x => x.OwnerId.Equals(ownerId)).AsQueryable());
            modelMock.Setup(x => x.GetByFilter(x => newProperty.Owners.Contains(x.OwnerId)))
             .Returns(_ownerList.Where(x => newProperty.Owners.Contains(x.OwnerId)).AsQueryable());

            return modelMock;
        }

        #region Util

        /// <summary>
        /// Get a Owner DDD like service
        /// </summary>
        /// <param name="ownerId">Owner Id</param>
        /// <param name="newProperty">Property request to create</param>
        /// <returns>Owner <see cref="Owner"/></returns>
        public Owner GetOwnerService(Guid ownerId, CreatePropertyBuildingRequest newProperty)
        {
            _ownerId = ownerId; 
            _name = "Cristian Moreno";
            _ownerList = new List<Owner>() {
                new Owner() { OwnerId = _ownerId, Name=_name, Address="Calle 123 # 23 a 25", Photo="https://www.myphoto.com", Birthday=new DateTime(1990,1,1,11,11,11, DateTimeKind.Utc) }
            };

            var modelMock = CreateMock(_name, _ownerId, newProperty);
            var mockObject = modelMock.Object;
            Owner ownerService = new(mockObject);
            ownerService.OwnerId = _ownerId;
            ownerService.Name = _name;
            return ownerService;
        }

        // TODO: The tests would go here but they are not carried out since it is not part of the test to manage the Owners

        #endregion
    }
}
