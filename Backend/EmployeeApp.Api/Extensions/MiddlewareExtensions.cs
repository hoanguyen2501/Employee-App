using EmployeeApp.Api.Middlewares;

namespace EmployeeApp.Api.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseApiMiddleware(this IApplicationBuilder app)
        {

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseCors(builder =>
                builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("http://localhost:4200"));

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<TransactionMiddleware>();

            return app;
        }
    }
}
