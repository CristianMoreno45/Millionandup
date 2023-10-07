#region Using
using Microsoft.EntityFrameworkCore;
using Millionandup.MsProperty.Api.Endpoints;
using Millionandup.MsProperty.Infrastructure.Repository.Contexts;
using Microsoft.Extensions.Logging.Console;
using Millionandup.Framework.Extensions.Program;
using Millionandup.MsProperty.Api;
#endregion

var builder = WebApplication.CreateBuilder(args);

// Configuration of Appsettings.json
builder.Host.SetAppSettings(builder.Environment.EnvironmentName);
// Add Cors fisical Policy
builder.Services.SetPhysicalPolicy("MyPolicy");
// Add Logger
using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSimpleConsole(i => i.ColorBehavior = LoggerColorBehavior.Disabled);
});
ILogger logger = loggerFactory.CreateLogger<Program>();
builder.Services.AddSingleton(logger);

IConfiguration configuration = builder.Configuration;
IServiceCollection services = builder.Services;

// Add db Context
services.AddDbContext<PropertyContext>(opt =>
{
    opt.UseSqlServer(configuration["DataBaseSettings:StringConnection"]);
});

//Add behaviors
services
    .AddBearerAuthentication(configuration["NetConnections:identityServer:host"] ?? "") // Add authentication                                                                  
    .AddScopesPolicy("Owner", new List<string>() { "Add", "Get" }) // Add Owner Scopes policy  
    .AddScopesPolicy("Property", new List<string>() { "CreatePropertyBuilding", "AddImageFromProperty", "ChangePrice", "UpdateProperty", "ListPropertyWithFilters" }) // Add Property Scopes policy  
    .AddEndpointsApiExplorer() // Add endppints
    .AddSwaggerGen()// Add Swagger
    .SolveDependencyInjection()// Registry Dependency injection
    .ConfigureJsonPreferences();// Add Json configuration

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection().UseCors().UseAuthentication().UseAuthorization(); app.AddPropertyEndpoints();
app.AddOwnerEndpoints();
app.Run();
