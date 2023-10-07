using Millionandup.MsProperty.Domain.AggregatesModel;
using System.Linq.Expressions;

namespace Millionandup.MsProperty.Domain.Interfaces
{
    /// <summary>
    /// Owner contract
    /// </summary>
    public interface IOwner
    {
        #region Properties
        /// <summary>
        /// Owner Id
        /// </summary>
        Guid OwnerId { get; set; }

        /// <summary>
        /// Owner name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Owner Address
        /// </summary>
        string Address { get; set; }

        /// <summary>
        /// Owner Photo URL
        /// </summary>
        string Photo { get; set; }

        /// <summary>
        /// Owner Birthday
        /// </summary>
        DateTime Birthday { get; set; }

        /// <summary>
        /// Property owners
        /// </summary>
        public ICollection<Property> Property { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Method that allows adding an entity
        /// </summary>
        /// <param name="entity">Entity to add</param>
        /// <returns>Task</returns>
        Owner Add(Owner entity);

        /// <summary>
        /// Method that allows deleting an entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        Owner Delete(Owner entity);

        /// <summary>
        /// Method that obtains all database objects according to a filter
        /// </summary>
        /// <param name="filter">Individual properties of owner</param>
        /// <returns>IQueryable of the Entity</returns>
        List<Owner> GetByFilter(Owner filter);

        /// <summary>
        /// Method that obtains all database objects according to a filter
        /// </summary>
        /// <param name="filter">Lamda expresion to filter</param>
        /// <returns>IQueryable of the Entity</returns>
        List<Owner> GetByFilter(Expression<Func<Owner, bool>> filter);

        #endregion
    }
}
