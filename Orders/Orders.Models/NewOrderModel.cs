
using System;

namespace Orders.Models
{
    public class NewOrderModel:INewOrderModel
    {
        public Guid CorrelationId { get; set; }
        public int OrderId { get; set; }
    }
}
