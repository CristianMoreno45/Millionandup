using Millionandup.Framework;
using Millionandup.MsProperty.Domain.AggregatesModel;
using Millionandup.MsProperty.Domain.Interfaces;
using Millionandup.MsProperty.Infrastructure.Repository.Contexts;

namespace Millionandup.MsProperty.Infrastructure.Repository.Entities
{
    /// <summary>
    /// Data base model of Property
    /// </summary>
    public class PropertyModel : StandardModelImplementation<Property>, IPropertyModel
    {

        #region Properties & Attributes
        #endregion

        #region Class constructors

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="context">Data base context</param>
        public PropertyModel(PropertyContext context) : base(context)
        {
            _db = context;
            _model = _db.Set<Property>();
        }
        #endregion

        #region Methods

        #endregion
    }
}
