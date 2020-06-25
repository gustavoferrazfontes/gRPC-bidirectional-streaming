using Ecommerce;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Utils;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrderManagementClient
{
    class Program
    {
         static async Task Main(string[] args)
        {
            var channel = new Channel("192.168.0.5", 5001, ChannelCredentials.Insecure);
            var client = new OrderManagementService.OrderManagementServiceClient(channel);

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            using var streamCall = client.processOrders();

            for (int i = 0; i < 15; i++)
            {
                if (i % 2 == 0)
                    await streamCall.RequestStream.WriteAsync( new StringValue { Value = "sp" });
                else
                   await  streamCall.RequestStream.WriteAsync( new StringValue { Value = "rj" });

            }

            await streamCall.RequestStream.CompleteAsync();

            try
            {
                await streamCall.ResponseStream.ForEachAsync( item  =>
                     Task.Run(() => {
                         Console.WriteLine($"Id:{item.Id}");
                         item.OrdersList.ToList().ForEach(i => Console.WriteLine(i.Id));
                     }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            Console.ReadKey();

        }
    }
}
