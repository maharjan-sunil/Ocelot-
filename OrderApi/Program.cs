using JwtAuthenticationManager;
using OrderAPI.Data;
using OrderAPI.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


//Add Logger 
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

logger.Information("OrderApiService executed.");

// Add services to the container.
builder.Services.AddScoped<IOrderService, OrderService>();
//Add DB Context
builder.Services.AddDbContext<DbContextClass>();

builder.Services.AddControllers();
builder.Services.AddCustomJwtAuthentication();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
