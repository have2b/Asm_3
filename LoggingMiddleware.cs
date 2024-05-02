using Serilog;
using System.Text;

namespace Asm_3
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                var request = httpContext.Request;
                var bodyContent = await new StreamReader(request.Body).ReadToEndAsync();

                
                var logBuilder = new StringBuilder();
                logBuilder.AppendLine("{");
                logBuilder.AppendLine($"Scheme: [{request.Scheme}]");
                logBuilder.AppendLine($"Host: [{request.Host}]");
                logBuilder.AppendLine($"Path: [{request.Path}]");
                logBuilder.AppendLine($"Query String: [{request.QueryString}]");
                logBuilder.AppendLine($"Body: [{bodyContent}]");
                logBuilder.AppendLine("}");
                
                Log.Information( logBuilder.ToString() );
            }
            catch
            {
                Log.Error($"There are something wrong in {nameof(LoggingMiddleware)}");
            }
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}
