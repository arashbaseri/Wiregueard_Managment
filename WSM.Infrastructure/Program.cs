using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using WSM.Application.Interfaces;
using WSM.Infrastructure;
using Serilog;
using Telegram.Bot.Types;
using System.IO;
using WSM.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
if (!Directory.Exists(logDirectory))
{
    Directory.CreateDirectory(logDirectory);
}

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
//    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
// add this part so logger in other projects works.
builder.Logging.ClearProviders(); // Remove default providers
builder.Logging.AddSerilog();    // Add Serilog as the provider



// Add services to the container.
builder.Services.AddControllers();
builder.Services.ConfigureTelegramBotMvc();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    // Additional Swagger configuration can go here
});
builder.Services.AddInfrastructure(builder.Configuration);
// AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Register AppDbContext for PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


// Set up the webhook
var botService = app.Services.GetRequiredService<ITelegramBotService>();
await botService.SetWebhookAsync(builder.Configuration["TelegramBot:WebhookUrl"]);

Log.Information("\nStarting up\n");
app.Run();
