using Ecommerce;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Server
{
    public class OrderManagementServiceImpl : OrderManagementService.OrderManagementServiceBase
    {

        public async override Task processOrders(IAsyncStreamReader<StringValue> requestStream, IServerStreamWriter<CombinedShipment> responseStream, ServerCallContext context)
        {
         
            RepeatedField<Order> queue= new RepeatedField<Order>();

            while (await requestStream.MoveNext())
            {
                if (requestStream.Current.Value == "sp")
                {
                    queue.Add(new Order { Id = Guid.NewGuid().ToString(), Destination = "SP" });
                }
            }

            var sp_box = new CombinedShipment
            {
                Id = "sp_box",
                Status = "sending",
            };

            sp_box.OrdersList.AddRange(queue.Where(q => q.Destination == "SP"));

            await responseStream.WriteAsync(sp_box);
        }
    }
}
