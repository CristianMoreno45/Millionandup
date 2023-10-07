using Millionandup.MsProperty.Api.EndpointsHandlers;

namespace Millionandup.MsProperty.Api.Endpoints
{
    /// <summary>
    /// Class in charge of managing endpoints
    /// </summary>
    public static partial class OwnerApi
    {
        /// <summary>
        /// API Base Route
        /// </summary>
        private const string API_BASE_PATH = "api/Owner/v1";

        /// <summary>
        /// Get the full path of the api
        /// </summary>
        /// <param name="app">WebApplication</param>
        public static void AddOwnerEndpoints(this WebApplication app)
        {
            app.MapPost($"{API_BASE_PATH}/Add", OwnerHandlers.Add).RequireAuthorization("Add");
            app.MapPost($"{API_BASE_PATH}/Get", OwnerHandlers.Get).RequireAuthorization("Get");
        }
    }
}
