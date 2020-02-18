using System;
using System.Threading.Tasks;
using Finances.App.Publishers;
using Finances.Models;
using MassTransit;

namespace Finances.App.Consumers
{
    public class NewOrderConsumer : IConsumer<Orders.Models.INewOrderModel>
    {
        private IOrderPublisher OrderPublisher { get; }

        public NewOrderConsumer(IOrderPublisher orderPublisher)
        {
            OrderPublisher = orderPublisher;
        }

        public async Task Consume(ConsumeContext<Orders.Models.INewOrderModel> context)
        {
            await Console.Out.WriteLineAsync($"Received new order: {context.Message.OrderId}");
            await Task.Delay(3000);
            await OrderPublisher.OrderPaid(new OrderPaidModel
            {
                OrderId = context.Message.OrderId,
                CorrelationId = context.Message.CorrelationId
            });
        }
    }
}
