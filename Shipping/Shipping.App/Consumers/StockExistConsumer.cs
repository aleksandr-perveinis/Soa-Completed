using MassTransit;
using System;
using System.Threading.Tasks;

namespace Shipping.App.Consumers
{
    public class StockExistConsumer : IConsumer<Warehouses.Models.IStockExistModel>
    {
        public async Task Consume(ConsumeContext<Warehouses.Models.IStockExistModel> context)
        {
            await Console.Out.WriteLineAsync($"Stock Exist for order: {context.Message.OrderId}");
        }
    }
}
