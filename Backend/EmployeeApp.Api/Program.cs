using EmployeeApp.Api.Extensions;
using EmployeeApp.DAL.DataAccess;
using EmployeeApp.DAL.DataAccess.Seeding;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

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
                        .AddApplicationServices(builder.Configuration);
    }

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseApiMiddleware();

    app.MapControllers();

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<EmployeeAppDbContext>();
        await context.Database.MigrateAsync();
        await Seed.SeedCompanies(context);
    }
    catch (Exception exception)
    {
        logger.Error(exception, "An error occurred during migration");
        throw;
    }

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
