﻿using MassTransit;
using System;
using System.Threading.Tasks;
using MassTransit.Saga;
using Test.StateMachine;

namespace Test
{
    public class Program
    {
        public static async Task Main()
        {
            var config = Utils.Configuration.GetServiceBusConfiguration();
            Console.Title = config.QueueName;

            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = Utils.HostFactory.CreateHost(cfg);

                cfg.ReceiveEndpoint(config.QueueName + "Endpoint", ep =>
                  {

                      ep.Handler<Models.Message>(context => Console.Out.WriteLineAsync($"Received from Endpoint: {context.Message.Text}"));
                  });

                cfg.ReceiveEndpoint(config.QueueName + "Subscriber", ep =>
                   {
                       ep.Consumer<Consumers.TestConsumer>();
                   });
            });
            var client = CreateRequestClient(bus, $"{config.ServerName}/{config.VirtualHost}/{config.QueueName}Subscriber");

            var stateMachine = new TestStateMachine();
            var repository = new InMemorySagaRepository<TestState>();

            bus.ConnectReceiveEndpoint(config.QueueName, cfg =>
            {
                cfg.StateMachineSaga<TestState>(stateMachine, repository);
            });

            EndpointConvention.Map<Models.Message>(new Uri($"{config.ServerName}/{config.VirtualHost}/{config.QueueName}Endpoint"));

            await bus.StartAsync();

            await bus.Send(new Models.Message { Text = "Hello send" });

            await bus.Publish(new Models.Message { Text = "Hello publish" });


            var response = await client.Request(new Models.Message
            {
                CorrelationId = Guid.NewGuid(),
                Text = "RPC Message"
            });

            Console.WriteLine("RPC Result: {0}", response.ResponseText);

            Console.WriteLine("Press any key to exit");
            await Task.Run(Console.ReadKey);

            await bus.StopAsync();
        }
        static IRequestClient<Models.IMessage, Models.IMessageResponse> CreateRequestClient(IBusControl busControl, string serviceAddress)
        {
            IRequestClient<Models.IMessage, Models.IMessageResponse> client = busControl.CreateRequestClient<Models.IMessage, Models.IMessageResponse>(new Uri(serviceAddress), TimeSpan.FromSeconds(3));

            return client;
        }

    }
}
