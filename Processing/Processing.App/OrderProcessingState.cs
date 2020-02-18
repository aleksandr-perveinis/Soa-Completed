using Automatonymous;
using System;

namespace Processing.App
{
    public class OrderProcessingState : SagaStateMachineInstance
    {
        public OrderProcessingState(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get; set; }
        public CompositeEventStatus CompositeStatus { get; set; }
        public string State { get; set; }

        public int OrderId { get; set; }
        public DateTimeOffset? OrderSubmitted { get; set; }
        public DateTimeOffset? PaymentProcessed { get; set; }
        public DateTimeOffset? StockReserved { get; set; }

    }
}
