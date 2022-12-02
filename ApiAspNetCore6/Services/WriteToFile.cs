namespace ApiAspNetCore6.Services
{
    public class WriteToFile : IHostedService
    {
        private readonly IWebHostEnvironment env;
        private readonly string filename = "file1.txt";
        private Timer timer;

        public WriteToFile(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            this.Write($"Proceso iniciado... {DateTime.Now}");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Dispose();
            this.Write($"Proceso finalizado... {DateTime.Now}");
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Write($"Proceso en ejecución: {DateTime.Now}");
        }

        private void Write(string message)
        {
            var path = $@"{env.ContentRootPath}\wwwroot\{filename}";
            using(StreamWriter sw = new StreamWriter(path, append: true))
            {
                sw.WriteLine(message);
            }
        }
    }
}
