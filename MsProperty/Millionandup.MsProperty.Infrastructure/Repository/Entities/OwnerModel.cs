using Millionandup.Framework;
using Millionandup.MsProperty.Domain.AggregatesModel;
using Millionandup.MsProperty.Domain.Interfaces;
using Millionandup.MsProperty.Infrastructure.Repository.Contexts;

namespace Millionandup.MsProperty.Infrastructure.Repository.Entities
{
    /// <summary>
    /// Data base model of Owner
    /// </summary>
    public class OwnerModel : StandardModelImplementation<Owner>, IOwnerModel
    {

        #region Properties & Attributes
        #endregion

        #region Class constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="context">Data base context</param>
        public OwnerModel(PropertyContext context) : base(context)
        {
            _db = context;
            _model = _db.Set<Owner>();
        }
        #endregion

        #region Methods

        #endregion
    }
}
