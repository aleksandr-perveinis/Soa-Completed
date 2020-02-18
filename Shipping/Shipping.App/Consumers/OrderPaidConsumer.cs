using MassTransit;
using System;
using System.Threading.Tasks;

namespace Shipping.App.Consumers
{
    public class OrderPaidConsumer : IConsumer<Finances.Models.IOrderPaidModel>
    {
        public async Task Consume(ConsumeContext<Finances.Models.IOrderPaidModel> context)
        {
            await Console.Out.WriteLineAsync($"Order {context.Message.OrderId} is paid");
        }
    }
}
