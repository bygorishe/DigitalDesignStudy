using MyInsta.Common.Consts;
using MyInsta.Common.Exceptions;
using MyInsta.Api.Services;
using MyInsta.Common.Extentions;
using MyInsta.DAL.Entities;

namespace MyInsta.Api.Middlewares
{
    public class StopDdosMiddleware
    {
        private readonly RequestDelegate _next;

        public StopDdosMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DDOS_Guard guard)
        {
            var headerAuth = context.Request.Headers.Authorization;
            try
            {
                guard.CheckRequest(headerAuth);
                await _next(context);
            }
            catch (TooManyRequestException)
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsJsonAsync("too many Requests, allowed 10 request per second");
            }
        }
    }

    public static class StopDdosMiddlewareMiddlewareExtensions
    {
        public static IApplicationBuilder UseAntiDdosCustom(
            this IApplicationBuilder builder)
            => builder.UseMiddleware<StopDdosMiddleware>();
    }
}