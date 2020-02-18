
using System;

namespace Warehouses.Models
{
    public interface IStockExistModel
    {
        Guid CorrelationId { get; set; }
        int OrderId { get; set; }
    }
}
