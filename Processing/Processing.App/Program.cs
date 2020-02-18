using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Saga;
using Microsoft.Extensions.DependencyInjection;
using Processing.App.Publishers;

namespace Processing.App
{
    class Program
    {
        public static async Task Main()
        {
            var services = new ServiceCollection()
                .AddSingleton<IOrderPublisher, OrderPublisher>()
                .AddSingleton<OrderProcessorStateMachine, OrderProcessorStateMachine>();

            var config = Utils.Configuration.GetServiceBusConfiguration();
            Console.Title = config.QueueName;

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                Utils.HostFactory.CreateHost(cfg);
            }));

            services.AddSingleton<ISagaRepository<OrderProcessingState>, InMemorySagaRepository<OrderProcessingState>>(x => new InMemorySagaRepository<OrderProcessingState>());

            services.AddMassTransit(cfg =>
            {
                cfg.AddSagaStateMachine<OrderProcessorStateMachine, OrderProcessingState>();
            });
            var serviceProvider = services.BuildServiceProvider();

            var bus = serviceProvider.GetRequiredService<IBusControl>();
            bus.ConnectReceiveEndpoint(config.QueueName, cfg =>
            {
                cfg.StateMachineSaga<OrderProcessingState>(serviceProvider);
            });

            await bus.StartAsync();

            Console.WriteLine("Press any key to exit");

            await Task.Run(Console.ReadKey);

            await bus.StopAsync();
        }
    }
}
