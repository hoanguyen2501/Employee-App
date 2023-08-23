using EmployeeApp.Api.Middlewares;

namespace EmployeeApp.Api.Extensions
{
    public static class MiddlewareExtensions
    {
        public static WebApplication UseApiMiddleware(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseMiddleware<TransactionMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
