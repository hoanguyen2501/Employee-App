using EmployeeApp.Domain.UOW;
using Microsoft.EntityFrameworkCore.Storage;

namespace EmployeeApp.Api.Middlewares
{
    public class TransactionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TransactionMiddleware> _logger;

        public TransactionMiddleware(RequestDelegate next, ILogger<TransactionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, IUnitOfWork unitOfWork)
        {
            if (httpContext.Request.Method.Equals("GET", StringComparison.CurrentCultureIgnoreCase))
            {
                await _next(httpContext);
                return;
            }
            else
            {
                using IDbContextTransaction transaction = await unitOfWork.CreateTransaction();

                await _next(httpContext);
                if (httpContext.Response.StatusCode == 200)
                {
                    await transaction.CommitAsync();
                    _logger.LogInformation("Transaction was committed successfully");
                }
                else
                {
                    await transaction.RollbackAsync();
                    _logger.LogError("Somethings went wrong during committing transaction");
                }

            }
        }
    }
}
