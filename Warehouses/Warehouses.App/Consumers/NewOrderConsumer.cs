using System;
using System.Threading.Tasks;
using MassTransit;
using Warehouses.App.Publishers;
using Warehouses.Models;

namespace Warehouses.App.Consumers
{
    public class NewOrderConsumer : IConsumer<Orders.Models.INewOrderModel>
    {
       private IStockPublisher StockPublisher { get; }
       
       public NewOrderConsumer(IStockPublisher stockPublisher)
        {
            StockPublisher = stockPublisher;
        }

        public async Task Consume(ConsumeContext<Orders.Models.INewOrderModel> context)
        {
            await Console.Out.WriteLineAsync($"Received new order: {context.Message.OrderId}");
            await Task.Delay(2000);

            await StockPublisher.StockExist(new StockExistModel
            {
                CorrelationId = context.Message.CorrelationId,
                OrderId = context.Message.OrderId
            });
        }
    }
}
