using System;
using System.Threading.Tasks;
using MassTransit;
using Warehouses.Models;

namespace Warehouses.App.Publishers
{
    public class StockPublisher : IStockPublisher
    {
        private IBusControl Bus { get;  }

        public StockPublisher(IBusControl bus)
        {
            Bus = bus;
        }

        public async Task StockExist(IStockExistModel model)
        {
            await Bus.Publish(model);
            await Console.Out.WriteLineAsync($"Stock exists for order : {model.OrderId}");

        }
    }
}
