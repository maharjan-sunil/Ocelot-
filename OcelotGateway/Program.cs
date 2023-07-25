using Ocelot.DependencyInjection;
using JwtAuthenticationManager;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

logger.Information("Ocelot gateway executed.");

// Add services to the container.

/*The main functionality of an Ocelot API Gateway is to take incoming HTTP requests and
forward them on to a downstream service, currently as another HTTP request.
Ocelot's describes the routing of one request to another as a ReRoute.*/

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddCustomJwtAuthentication();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.UseOcelot();

app.Run();
