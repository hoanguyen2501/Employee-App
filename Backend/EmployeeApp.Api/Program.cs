using EmployeeApp.Api.Extensions;
using NLog;
using NLog.Web;

Logger logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Init main");

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    {

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add NLog
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();

        builder.Services.AddJwtIdentity(builder.Configuration)
                        .AddDependencyInjection()
                        .AddApplicationServices(builder.Configuration)
                        .AddControlerAsServiceExtension();
    }

    WebApplication app = builder.Build();

    app.UseApiMiddleware();

    await app.SeedingExtensionsAsync(logger);

    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Some errors occurred during starting program");
    throw;
}
finally
{
    LogManager.Shutdown();
}
