namespace Millionandup.Framework.DTO
{
    /// <summary>
    /// Data transfer object: standard response
    /// </summary>
    /// <typeparam name="TData">Class data type</typeparam>
    public class ResponseBase<TData>
    {
        /// <summary>
        /// General status of the request (internal)
        /// </summary>
        private HeaderResponseBase _header;

        /// <summary>
        /// General status of the request
        /// </summary>
        public HeaderResponseBase Header
        {
            get
            {
                if (_header == null)
                    _header = new HeaderResponseBase();
                return _header;
            }
            set
            {
                _header = value;
            }
        }

        /// <summary>
        /// Concreate data response
        /// </summary>
        public TData? Data { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public ResponseBase()
        {
            _header = new HeaderResponseBase();
        }
    }
}
