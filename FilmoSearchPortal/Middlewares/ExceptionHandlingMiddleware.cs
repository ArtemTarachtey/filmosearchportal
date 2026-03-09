using System.Net;
using System.Text.Json;

namespace FilmoSearchPortal.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger) { 
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
                _logger.LogError(e, "Error  {Message}", e.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = new { StatusCode = context.Response.StatusCode, Message = "There was an error with the external device. Please try again later." };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
