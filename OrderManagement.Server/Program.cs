using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace OrderManagement.Server
{
    class Program
    {
        async static Task Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService<TimedHostedService>();
                })
                .UseConsoleLifetime()
                .Build();
            await hostBuilder.RunAsync();
        }
    }
}
