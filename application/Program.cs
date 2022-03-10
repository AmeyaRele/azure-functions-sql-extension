using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.Sql;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SqlExample
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .UseEnvironment("Development")
                .ConfigureWebJobs(webJobsBuilder => webJobsBuilder.AddSql())
                .ConfigureLogging(loggingBuilder => loggingBuilder.SetMinimumLevel(LogLevel.Debug).AddConsole())
                .UseConsoleLifetime();

            using var host = hostBuilder.Build();
            await host.RunAsync();
        }
    }
}