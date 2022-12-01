using ApiAspNetCore6;

var builder = WebApplication.CreateBuilder(args);

//OBJ Startup
var startup = new Startup(builder.Configuration);

//Services
startup.ConfigureServices(builder.Services);

var app = builder.Build();

//logger
var loggerService = (ILogger<Startup>)app.Services.GetService(typeof(ILogger<Startup>));

//Middlewares
startup.Configure(app, app.Environment, loggerService);
//startup.Configure(app, app.Environment); //original

app.Run();