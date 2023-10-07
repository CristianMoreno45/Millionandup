using Microsoft.AspNetCore.Mvc;
using Millionandup.Framework.Domain.Exceptions;
using Millionandup.Framework.DTO;
using Millionandup.Framework.Exceptions;
using Millionandup.MsProperty.Domain.AggregatesModel;
using Millionandup.MsProperty.Domain.Interfaces;
using System.Net;

namespace Millionandup.MsProperty.Api.EndpointsHandlers
{
    /// <summary>
    /// Owner minimal API handler
    /// </summary>
    public static partial class OwnerHandlers
    {
        /// <summary>
        /// Method that allows adding an entity
        /// </summary>
        /// <param name="ownerService">[Dependency injection] Service DDD of Owner</param>
        /// <param name="logger">[Dependency injection] logger</param>
        /// <param name="owner">POCO Of Owner to Create</param>
        /// <returns>Created Owner</returns>
        internal static async Task<IResult> Add([FromServices] IOwner ownerService, [FromServices] ILogger logger, [FromBody] Owner owner)
        {
            try
            {
                return Results.Json(ownerService.Add(owner).AsResponseDTO());
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
        /// Method that allows geting an entity
        /// </summary>
        /// <param name="ownerService">[Dependency injection] Service DDD of Owner</param>
        /// <param name="logger">[Dependency injection] logger</param>
        /// <param name="filter">POCO Of Owner to filter</param>
        /// <returns>Owners</returns>
        internal static async Task<IResult> Get([FromServices] IOwner ownerService, [FromServices] ILogger logger, [FromBody] Owner filter)
        {
            try
            {
                return Results.Json(ownerService.GetByFilter(filter).AsResponseDTO());
            }
            catch (InvalidModelException ex)
            {
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
