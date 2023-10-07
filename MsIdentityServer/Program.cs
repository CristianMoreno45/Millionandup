
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServerHost.Quickstart.UI;
using Millionandup.Framework.Extensions.Program;
using Millionandup.MsIdentityServer.Infrastructure;
using Millionandup.MsIdentityServer.AggregatesModel;
using Millionandup.MsIdentityServer;

var builder = WebApplication.CreateBuilder(args);


builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add Cors fisical Policy
builder.Services.SetPhysicalPolicy("MyPolicy");

// Add services to the container.
builder.Services.AddControllersWithViews();
// Configuration of Appsettings.json
builder.Host.SetAppSettings(builder.Environment.EnvironmentName);

var migrationsAssembly = "Millionandup.MsIdentityServer";
var config = builder.Configuration;
// Add DbContext
builder.Services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(config["DataBaseSettings:StringConnection"]));

// Add repositories in controllers with dependency injection
builder.Services.AddScoped<IAccount, Account>();

// Enable IS4Endpoints
builder.Services.AddLocalApiAuthentication();
// *******************************************************
// Implementation of identityServer 4
// *******************************************************
// Ads Identity Provider
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();


// View It's work in https://localhost:44333/.well-known/openid-configuration
// Sql implementation here: https://docs.identityserver.io/en/release/quickstarts/8_entity_framework.html
builder.Services.AddIdentityServer()
     .AddAspNetIdentity<AppUser>()
     .AddDeveloperSigningCredential() // Create Temporal Keys, allow Dev Certificates TODO: Only Develop environment
     .AddInMemoryClients(Config.Clients(config["NetConnections:identityServer:host"])) // AppClient list with permissions
     .AddInMemoryIdentityResources(Config.IdentityResources()) // Resource of identity (email, UserId, etc)
                                                               //.AddInMemoryApiResources(Config.ApiResources) // Resources to be protected
     .AddInMemoryApiScopes(Config.ApiScopes()) // Resources to be protected
     .AddTestUsers(TestUsers.Users) // Model of Tests users TODO: Only Develop environment
     .AddConfigurationStore(options =>   // this adds the config data from DB (clients, resources)
                 {
                     options.ConfigureDbContext = builder =>
                                     builder.UseSqlServer(config["DataBaseSettings:StringConnection"], sql => sql.MigrationsAssembly(migrationsAssembly));
                 })
     .AddOperationalStore(options =>  // this adds the operational data from DB (codes, tokens, consents)
                 {
                     options.ConfigureDbContext = builder =>
                         builder.UseSqlServer(config["DataBaseSettings:StringConnection"], sql => sql.MigrationsAssembly(migrationsAssembly));

                     // this enables automatic token cleanup. this is optional.
                     options.EnableTokenCleanup = true;
                     options.TokenCleanupInterval = 30;
                 });

// Add Swagger
builder.Services.AddSwaggerGen();


// *******************************************************
// END Implementation of identityServer 4
// *******************************************************



var app = builder.Build();
// *******************************************************
// Run first Identyserver migration
// *******************************************************
using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

    var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
    context.Database.Migrate();
    // migrate config.cs settings into Db

    var clients = Config.Clients((config["NetConnections:identityServer:host"])).Where(x => !context.Clients.Select(y => y.ClientId).Contains(x.ClientId));
    if (clients.Any())

    {
        foreach (var client in clients)
        {
            context.Clients.Add(client.ToEntity());
        }
        context.SaveChanges();
    }
    var identityResources = Config.IdentityResources().Where(x => !context.IdentityResources.Select(y => y.Name).Contains(x.Name));
    if (identityResources.Any())
    {
        foreach (var resource in identityResources)
        {
            context.IdentityResources.Add(resource.ToEntity());
        }
        context.SaveChanges();
    }

    var apiResources = Config.ApiResources().Where(x => !context.ApiResources.Select(y => y.Name).Contains(x.Name));
    if (apiResources.Any())
    {
        foreach (var resource in apiResources)
        {
            context.ApiResources.Add(resource.ToEntity());
        }
        context.SaveChanges();
    }
    var scopes = Config.ApiScopes().Where(x => !context.ApiScopes.Select(y => y.Name).Contains(x.Name));
    if (scopes.Any())
    {
        foreach (var scope in scopes)
        {
            context.ApiScopes.Add(scope.ToEntity());
        }
    }

    context.SaveChanges();

}

// Use policy
app.UseCors("MyPolicy");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// *******************************************************
// END Run first Identyserver migration
// *******************************************************

app.UseStaticFiles();
app.UseRouting();
// Enable IS4Endpoints with security
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();

    endpoints.MapGet("/", context =>
    {
        return Task.Run(() => context.Response.Redirect("/Home/Index"));
    });
});

app.UseIdentityServer();

app.Run();
