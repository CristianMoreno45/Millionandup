using Millionandup.MsProperty.Api.EndpointsHandlers;

namespace Millionandup.MsProperty.Api.Endpoints
{
    /// <summary>
    /// Class in charge of managing endpoints
    /// </summary>
    public static partial class PropertyApi
    {
        /// <summary>
        /// API Base Route
        /// </summary>
        private const string API_BASE_PATH = "api/Property/v1";

        /// <summary>
        /// Get the full path of the api
        /// </summary>
        /// <param name="app">WebApplication</param>
        public static void AddPropertyEndpoints(this WebApplication app)
        {
            app.MapPost($"{API_BASE_PATH}/CreatePropertyBuilding", PropertyHandlers.CreatePropertyBuilding);
            app.MapPost($"{API_BASE_PATH}/AddImageFromProperty", PropertyHandlers.AddImageFromProperty);
            app.MapPost($"{API_BASE_PATH}/ChangePrice", PropertyHandlers.ChangePrice);
            app.MapPost($"{API_BASE_PATH}/UpdateProperty", PropertyHandlers.UpdateProperty);
            app.MapPost($"{API_BASE_PATH}/ListPropertyWithFilters", PropertyHandlers.ListPropertyWithFilters);
        }
    }
}
