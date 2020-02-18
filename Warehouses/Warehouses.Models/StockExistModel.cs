
using System;

namespace Warehouses.Models
{
    public class StockExistModel:IStockExistModel
    {
        public Guid CorrelationId { get; set; }
        public int OrderId { get; set; }
    }
}
