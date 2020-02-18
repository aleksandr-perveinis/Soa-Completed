using MassTransit;
using System;
using System.Threading.Tasks;

namespace Shipping.App.Consumers
{
    public class OrderApprovedConsumer : IConsumer<Processing.Models.IOrderApprovedModel>
    {
        public async Task Consume(ConsumeContext<Processing.Models.IOrderApprovedModel> context)
        {
            await Console.Out.WriteLineAsync($"Order {context.Message.OrderId} is approved");
        }
    }
}
