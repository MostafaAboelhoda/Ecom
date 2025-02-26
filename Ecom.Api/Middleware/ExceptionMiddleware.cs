using Ecom.Api.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Ecom.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _RateLimitWindow = TimeSpan.FromSeconds(30);

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment environment, IMemoryCache memoryCache)
        {
            _next = next;
            _environment = environment;
            _memoryCache = memoryCache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                ApplySecurity(context);

                if (IsRequestAllowed(context) == false)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";
                    var response = new APIException
                        ((int)HttpStatusCode.TooManyRequests,
                        "Too Many Request. please try again later");
                    await context.Response.WriteAsJsonAsync(response);
                }
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var response = _environment.IsDevelopment() ?
                    new APIException(
                    (int)HttpStatusCode.InternalServerError,
                ex.Message,
                ex.StackTrace
                    ) : new APIException(
                    (int)HttpStatusCode.InternalServerError,
                ex.Message
                    );
                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);

            }
        }

        private bool IsRequestAllowed(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress.ToString();
            var cacheKey = $"Rate:{ip}";
            var dateNow = DateTime.Now;
            var (timeStamp, count) = _memoryCache.GetOrCreate(cacheKey,
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = _RateLimitWindow;
                    return (timeStamp: dateNow, count: 0);
                });

            if (dateNow - timeStamp < _RateLimitWindow)
            {
                if (count >= 8)
                {
                    return false;
                }
                _memoryCache.Set(cacheKey, (timeStamp, count += 1), _RateLimitWindow);
            }
            else
            {
                _memoryCache.Set(cacheKey, (timeStamp, count), _RateLimitWindow);

            }
            return true;
        }

        private void ApplySecurity(HttpContext context)
        {
            context.Response.Headers["x-content-type-Options"] = "nosniff";
            context.Response.Headers["x-XSS-protection"] = "1;mode=block";
            context.Response.Headers["x-Frame-Options"] = "DENY";
        }
    }
}
