using Microsoft.AspNetCore.Mvc;
using Millionandup.Framework.Domain.Exceptions;
using Millionandup.Framework.DTO;
using Millionandup.Framework.Exceptions;
using Millionandup.MsProperty.Domain.DTO;
using Millionandup.MsProperty.Domain.Interfaces;
using System.Net;

namespace Millionandup.MsProperty.Api.EndpointsHandlers
{
    /// <summary>
    /// Property minimal API handler
    /// </summary>
    public static partial class PropertyHandlers
    {

        /// <summary>
        /// API that allows you to create a property
        /// </summary>
        /// <param name="propertyService">[Dependency injection] Service DDD of Property</param>
        /// <param name="logger">[Dependency injection] logger</param>
        /// <param name="property">New property</param>
        /// <returns>Created property</returns>
        internal static async Task<IResult> CreatePropertyBuilding([FromServices] IProperty propertyService, [FromServices] ILogger logger, [FromBody] CreatePropertyBuildingRequest property)
        {
            try
            {
                return Results.Json(propertyService.CreatePropertyBuilding(property).AsResponseDTO());
            }
            catch (InvalidModelException ex)
            {
                logger.LogError(AppLogEvents.Error, ex, $"[{DateTime.UtcNow}] - {ex.Message}" );
                return Results.Json(false.AsResponseDTO(HttpStatusCode.BadRequest, ex.Message), statusCode: (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                logger.LogError(AppLogEvents.Error, ex, ex.Message);
                return Results.Json(false.AsResponseDTO(HttpStatusCode.InternalServerError, MessagesError.GENERAL_ERROR), statusCode: (int)HttpStatusCode.InternalServerError);
            }

        }

        /// <summary>
        /// API that Allows you to add an image to a property
        /// </summary>
        /// <param name="propertyService">[Dependency injection] Service DDD of Property</param>
        /// <param name="logger">[Dependency injection] logger</param>
        /// <param name="propertyImage">Image to attach</param>
        /// <returns>Updated Property Image</returns>
        internal static async Task<IResult> AddImageFromProperty([FromServices] IProperty propertyService, [FromServices] ILogger logger, [FromBody] AddImageFromPropertyRequest propertyImage)
        {
            try
            {
                return Results.Json(propertyService.AddImageFromProperty(propertyImage).AsResponseDTO());
            }
            catch (InvalidModelException ex)
            {
                logger.LogError(AppLogEvents.Error, ex, $"[{DateTime.UtcNow}] - {ex.Message}");
                return Results.Json(false.AsResponseDTO(HttpStatusCode.BadRequest, ex.Message), statusCode: (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                logger.LogError(AppLogEvents.Error, ex, $"[{DateTime.UtcNow}] - {ex.Message}");
                return Results.Json(false.AsResponseDTO(HttpStatusCode.InternalServerError, MessagesError.GENERAL_ERROR), statusCode: (int)HttpStatusCode.InternalServerError);
            }

        }

        /// <summary>
        /// API that allows you to change the price of a property.
        /// </summary>
        /// <param name="propertyService">[Dependency injection] Service DDD of Property</param>
        /// <param name="logger">[Dependency injection] logger</param>
        /// <param name="changePriceRequest">New price</param>
        /// <returns>Updated property</returns>
        internal static async Task<IResult> ChangePrice([FromServices] IProperty propertyService, [FromServices] ILogger logger, [FromBody] ChangePriceRequest changePriceRequest)
        {
            try
            {
                return Results.Json(propertyService.ChangePrice(changePriceRequest).AsResponseDTO());
            }
            catch (InvalidModelException ex)
            {
                logger.LogError(AppLogEvents.Error, ex, $"[{DateTime.UtcNow}] - {ex.Message}");
                return Results.Json(false.AsResponseDTO(HttpStatusCode.BadRequest, ex.Message), statusCode: (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                logger.LogError(AppLogEvents.Error, ex, $"[{DateTime.UtcNow}] - {ex.Message}");
                return Results.Json(false.AsResponseDTO(HttpStatusCode.InternalServerError, MessagesError.GENERAL_ERROR), statusCode: (int)HttpStatusCode.InternalServerError);
            }

        }

        /// <summary>
        /// API that allows you to change the data of a property.
        /// </summary>
        /// <param name="propertyService">[Dependency injection] Service DDD of Property</param>
        /// <param name="logger">[Dependency injection] logger</param>
        /// <param name="updatePropertyRequest">new data of property</param>
        /// <returns>Updated property</returns>
        internal static async Task<IResult> UpdateProperty([FromServices] IProperty propertyService, [FromServices] ILogger logger, [FromBody] UpdatePropertyRequest updatePropertyRequest)
        {
            try
            {
                return Results.Json(propertyService.UpdateProperty(updatePropertyRequest).AsResponseDTO());
            }
            catch (InvalidModelException ex)
            {
                logger.LogError(AppLogEvents.Error, ex, $"[{DateTime.UtcNow}] - {ex.Message}");
                return Results.Json(false.AsResponseDTO(HttpStatusCode.BadRequest, ex.Message), statusCode: (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                logger.LogError(AppLogEvents.Error, ex, $"[{DateTime.UtcNow}] - {ex.Message}");
                return Results.Json(false.AsResponseDTO(HttpStatusCode.InternalServerError, MessagesError.GENERAL_ERROR), statusCode: (int)HttpStatusCode.InternalServerError);
            }

        }

        /// <summary>
        /// API that allows extracting property information according to a filter
        /// </summary>
        /// <param name="propertyService">[Dependency injection] Service DDD of Property</param>
        /// <param name="logger">[Dependency injection] logger</param>
        /// <param name="filter">Property filter</param>
        /// <returns>List of properties</returns>
        internal static async Task<IResult> ListPropertyWithFilters([FromServices] IProperty propertyService, [FromServices] ILogger logger, [FromBody] PropertyFilter filter)
        {
            try
            {
                return Results.Json(propertyService.ListPropertyWithFilters(filter).AsResponseDTO());
            }
            catch (InvalidModelException ex)
            {
                logger.LogError(AppLogEvents.Error, ex, message: $"[{DateTime.UtcNow}] - {ex.Message}");
                return Results.Json(false.AsResponseDTO(HttpStatusCode.BadRequest, ex.Message), statusCode: (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                logger.LogError(AppLogEvents.Error, ex, $"[{DateTime.UtcNow}] - {ex.Message}");
                return Results.Json(false.AsResponseDTO(HttpStatusCode.InternalServerError, MessagesError.GENERAL_ERROR), statusCode: (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
