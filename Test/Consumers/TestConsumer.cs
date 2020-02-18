using MassTransit;
using System;
using System.Threading.Tasks;

namespace Test.Consumers
{
    public class TestConsumer : IConsumer<Models.IMessage>
    {
        public async Task Consume(ConsumeContext<Models.IMessage> context)
        {
            await Console.Out.WriteLineAsync($"Received in consumer: {context.Message.Text}");
            await context.RespondAsync(new Models.MessageResponse
            {
                CorrelationId = context.Message.CorrelationId,
                ResponseText = $"This is response for '{context.Message.Text}'"
            });
        }
    }
}
