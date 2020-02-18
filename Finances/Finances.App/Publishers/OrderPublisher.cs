using System;
using System.Threading.Tasks;
using Finances.Models;
using MassTransit;

namespace Finances.App.Publishers
{
    public class OrderPublisher : IOrderPublisher
    {
        private IBusControl Bus { get; }

        public OrderPublisher(IBusControl bus)
        {
            Bus = bus;
        }

        public async Task OrderPaid(IOrderPaidModel model)
        {
            await Bus.Publish(model);
            await Console.Out.WriteLineAsync($"Order {model.OrderId} is paid");

        }
    }
}
