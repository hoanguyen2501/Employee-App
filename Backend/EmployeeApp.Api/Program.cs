using EmployeeApp.Api.Extensions;
using EmployeeApp.Api.Middlewares;
using EmployeeApp.DAL.DataAccess;
using EmployeeApp.DAL.DataAccess.Seeding;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

{
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

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

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(builder =>
    builder.AllowAnyHeader()
        .AllowAnyMethod()
        .WithOrigins("http://localhost:4200"));

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<EmployeeAppDbContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedCompanies(context);
}
catch (System.Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();
