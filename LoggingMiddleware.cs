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
            var req = httpContext.Request;
            var filePath = "log_request.txt";

            // Create file if file doesn't exist
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            var bodyContent = string.Empty;

            // Check if request method is POST
            if (req.Method == HttpMethods.Post)
            {
                bodyContent = await new StreamReader(req.Body).ReadToEndAsync();
            }

            using StreamWriter writer = File.AppendText(filePath);
            writer.WriteLine(
                $"{{\nScheme: [{req.Scheme}]\nHost: [{req.Host}]\nPath: [{req.Path}]\nQuery String: [{req.QueryString}]\nBody: [{bodyContent}]\n}}\n"
            );
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
