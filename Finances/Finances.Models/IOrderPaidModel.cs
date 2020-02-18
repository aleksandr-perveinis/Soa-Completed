using System;

namespace Finances.Models
{
    public interface IOrderPaidModel
    {
        Guid CorrelationId { get; set; }
        int OrderId { get; set; }
    }
}
