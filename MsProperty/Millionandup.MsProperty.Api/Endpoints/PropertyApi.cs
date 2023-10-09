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
            app.MapPost($"{API_BASE_PATH}/CreatePropertyBuilding", PropertyHandlers.CreatePropertyBuilding).RequireAuthorization("CreatePropertyBuilding");
            app.MapPost($"{API_BASE_PATH}/AddImageFromProperty", PropertyHandlers.AddImageFromProperty).RequireAuthorization("AddImageFromProperty");
            app.MapPut($"{API_BASE_PATH}/ChangePrice", PropertyHandlers.ChangePrice).RequireAuthorization("ChangePrice");
            app.MapPut($"{API_BASE_PATH}/UpdateProperty", PropertyHandlers.UpdateProperty).RequireAuthorization("UpdateProperty");
            app.MapPost($"{API_BASE_PATH}/ListPropertyWithFilters", PropertyHandlers.ListPropertyWithFilters).RequireAuthorization("ListPropertyWithFilters");
        }
    }
}
