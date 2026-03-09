using FilmoSearchPortal.Middlewares;

namespace FilmoSearchPortal.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandling(this IApplicationBuilder app)
        {

            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
