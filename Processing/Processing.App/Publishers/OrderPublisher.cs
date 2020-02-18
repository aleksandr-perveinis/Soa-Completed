using System;
using System.Threading.Tasks;
using MassTransit;
using Processing.Models;

namespace Processing.App.Publishers
{
    public class OrderPublisher : IOrderPublisher
    {
        private IBusControl Bus { get; }

        public OrderPublisher(IBusControl bus)
        {
            Bus = bus;
        }

        public async Task OrderApproved(IOrderApprovedModel model)
        {
            await Bus.Publish(model);
            await Console.Out.WriteLineAsync($"Order {model.OrderId} is approved");

        }
    }
}
