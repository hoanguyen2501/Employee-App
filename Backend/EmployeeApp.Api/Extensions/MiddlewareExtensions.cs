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

            app.UseMiddleware<JwtMiddleware>();

            app.UseMiddleware<TransactionMiddleware>();

            //app.UseAuthentication();

            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.MapControllers();

            return app;
        }
    }
}
