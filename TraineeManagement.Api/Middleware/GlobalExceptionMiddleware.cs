using System.Net;
using System.Text.Json;
 
namespace TraineeManagement.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
 
        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
 
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An unhandled exception occurred.");
                await HandleExceptionAsync(context , e);

                // throw;
            }
        }
 
        private static async Task HandleExceptionAsync(HttpContext context , Exception exception)
        {
            context.Response.ContentType = "application/json";
            // context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string message = "An unexpected error occurred. Please try again later.";
            if (exception is BadHttpRequestException badRequestException)
            {
                statusCode = badRequestException.StatusCode;
                message = badRequestException.Message;
            }

            context.Response.StatusCode = statusCode;
 
            var response = new
            {
                statusCode = statusCode,
                message = message
            };
            
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    } 
}