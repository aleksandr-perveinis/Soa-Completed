
using System;

namespace Orders.Models
{
    public interface INewOrderModel
    {
        Guid CorrelationId { get; set; }
        int OrderId { get; set; }
    }
}
