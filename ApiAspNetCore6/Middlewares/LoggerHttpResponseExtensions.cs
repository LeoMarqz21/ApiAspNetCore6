namespace ApiAspNetCore6.Middlewares
{
    public static class LoggerHttpResponseExtensions
    {
        public static IApplicationBuilder UseLoggerHttpResponse(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoggerHttpResponse>();
        }
    }
}
