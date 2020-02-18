using MassTransit;
using System;
using System.Threading.Tasks;

namespace Orders.App
{
    class Program
    {
        private static IBusControl _bus;
        public static async Task Main()
        {
            var config = Utils.Configuration.GetServiceBusConfiguration();
            Console.Title = config.QueueName;

            _bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                Utils.HostFactory.CreateHost(cfg);
            });

            await _bus.StartAsync();

            while (true)
            {
                await SendOrder();

                Console.WriteLine("");
                Console.WriteLine("Press any to send new order, 'c' to cancel");
                var key = await Task.Run(Console.ReadKey);
                if (key.KeyChar == 'c')
                {
                    break;
                }
            }

            await _bus.StopAsync();
        }

        private static async Task SendOrder()
        {
            var order = new Models.NewOrderModel
            {
                CorrelationId = Guid.NewGuid(),
                OrderId = DateTime.Now.Second*1000 + DateTime.Now.Millisecond
            };
            await _bus.Publish<Models.INewOrderModel>(order);
            Console.WriteLine($"Order {order.OrderId} published");
        }
    }
}
