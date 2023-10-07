using Millionandup.Framework.Domain;
using Millionandup.Framework.Linq;
using Millionandup.MsProperty.Domain.Interfaces;
using System.Linq.Expressions;

namespace Millionandup.MsProperty.Domain.AggregatesModel
{
    /// <summary>
    /// Owner Business Model
    /// </summary>
    public class Owner : BaseDomain<Owner, IOwnerModel>, IOwner
    {
        #region Attributes

        /// <summary>
        /// Owner Id
        /// </summary>
        public Guid OwnerId { get; set; }

        /// <summary>
        /// Owner name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Owner Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Owner Photo URL
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// Owner Birthday
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// Property owners
        /// </summary>
        public ICollection<Property> Property { get; set; }

        #endregion

        /// <summary>
        /// Class constructor
        /// </summary>
        public Owner()
        {
            Name = "";
            Address = "";
            Photo = "";
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Owner(IOwnerModel repository) : this()
        {
            _repository = repository;
        }

        #region Methods
        /// <summary>
        /// Method that allows adding an entity
        /// </summary>
        /// <param name="entity">Entity to add</param>
        /// <returns>Task</returns>
        public Owner Add(Owner entity)
        {
            // TODO: The validations would go here but it is not applied since they are not part of the test.
            _repository.Add(entity);
            _repository.SaveChanges();
            return entity;
        }

        /// <summary>
        /// Method that allows deleting an entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        public Owner Delete(Owner entity)
        {
            // TODO: The validations would go here but it is not applied since they are not part of the test.
            _repository.Delete(entity);
            _repository.SaveChanges();
            return entity;
        }

        /// <summary>
        /// Method that obtains all database objects according to a filter
        /// </summary>
        /// <param name="filter">Individual properties of owner</param>
        /// <returns>IQueryable of the Entity</returns>
        public List<Owner> GetByFilter(Owner filter)
        {
            IQueryable<Owner> preliminarResult = _repository.GetAll();

            if (filter.OwnerId != Guid.Empty)
                preliminarResult = preliminarResult.Where(p => p.OwnerId == filter.OwnerId);

            if (!string.IsNullOrEmpty(filter.Name))
                preliminarResult = preliminarResult.Where(p => p.Name == filter.Name);

            if (filter.Birthday != new DateTime(1970, 0, 0, 0, 0, 0, DateTimeKind.Utc))
                preliminarResult = preliminarResult.Where(p => p.Birthday == filter.Birthday);

            return preliminarResult.ToList();

        }

        /// <summary>
        /// Method that obtains all database objects according to a filter
        /// </summary>
        /// <param name="filter">Lamda expresion to filter</param>
        /// <returns>IQueryable of the Entity</returns>
        public List<Owner> GetByFilter(Expression<Func<Owner, bool>> filter) => _repository.GetByFilter(filter).ToList();

        #endregion
    }
}