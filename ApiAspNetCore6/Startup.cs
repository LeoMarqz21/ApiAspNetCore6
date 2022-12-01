using ApiAspNetCore6.Middlewares;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace ApiAspNetCore6
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //Services
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            services.AddDbContext<ApplicationDbContext>((options) => {
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection"));
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        //Middlewares
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            //middleware - leomarqz
            //app.UseMiddleware<LoggerHttpResponse>();
            app.UseLoggerHttpResponse();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
