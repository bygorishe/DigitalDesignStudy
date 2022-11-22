namespace Api.Middlewares
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(ex.Message);
                //await context.Response.CompleteAsync();
            }
        }
    }
    public static class ErrorMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalErrorWrapper(
            this IApplicationBuilder builder)
            => builder.UseMiddleware<ErrorMiddleware>();
    }
}