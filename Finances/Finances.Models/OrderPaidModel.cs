using System;

namespace Finances.Models
{
    public class OrderPaidModel : IOrderPaidModel
    {
        public Guid CorrelationId { get; set; }
        public int OrderId { get; set; }
    }
}
