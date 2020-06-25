using Ecommerce;
using Grpc.Core;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderManagement.Server
{
    public class TimedHostedService : IHostedService
    {
        private Grpc.Core.Server _server;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                _server = new Grpc.Core.Server
                {
                    Ports =
                    {
                        new ServerPort("192.168.0.5", 5001, ServerCredentials.Insecure)
                    },
                    Services = 
                    {
                        OrderManagementService.BindService(new OrderManagementServiceImpl())
                    }
                    
                };

                _server.Start();

                Console.WriteLine("Order management Server is running");
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Server down");
            return Task.CompletedTask;
        }
    }
}
