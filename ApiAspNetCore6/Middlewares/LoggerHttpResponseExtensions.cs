namespace ApiAspNetCore6.Middlewares
{
    public static class LoggerHttpResponseExtensions
    {
        public static IApplicationBuilder UseLoggerHttpResponse(this IApplicationBuilder app)
        {
            //leomarqz
            return app.UseMiddleware<LoggerHttpResponse>();
        }

        public void Test()
        {

        }
    }
}
