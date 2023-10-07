using Microsoft.Extensions.DependencyInjection.Extensions;
using Millionandup.MsProperty.Domain.AggregatesModel;
using Millionandup.MsProperty.Domain.Interfaces;
using Millionandup.MsProperty.Domain.ModelValidation;
using Millionandup.MsProperty.Infrastructure.Repository.Entities;

namespace Millionandup.MsProperty.Api
{
    public static class LocalDependencyInjection
    {
        /// <summary>
        /// Resolve dependency injection
        /// </summary>
        /// <param name="service">Target</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection SolveDependencyInjection(this IServiceCollection service)
        {
            service.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            service.AddScoped<IProperty, Property>();
            service.AddScoped<IOwner, Owner>();
            service.AddScoped<IPropertyModel, PropertyModel>();
            service.AddScoped<IOwnerModel, OwnerModel>();
            service.AddScoped<FluentValidation.IValidator<Property>, PropertyValidator>();
            service.AddScoped<FluentValidation.IValidator<PropertyImage>, PropertyImageValidator>();
            return service;
        }
    }
}
