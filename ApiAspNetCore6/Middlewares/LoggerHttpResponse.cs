namespace ApiAspNetCore6.Middlewares
{
    public class LoggerHttpResponse
    {
        private readonly RequestDelegate next;
        private readonly ILogger<LoggerHttpResponse> logger;

        public LoggerHttpResponse(RequestDelegate next, ILogger<LoggerHttpResponse> logger)
        {
            //leomarqz...
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                var body = context.Response.Body;
                context.Response.Body = memoryStream;

                await next(context);

                memoryStream.Seek(0, SeekOrigin.Begin);
                string response = new StreamReader(memoryStream).ReadToEnd();
                memoryStream.Seek(0, SeekOrigin.Begin);

                await memoryStream.CopyToAsync(body);

                context.Response.Body = body;

                logger.LogInformation(response);
            }
        }
    }
}
