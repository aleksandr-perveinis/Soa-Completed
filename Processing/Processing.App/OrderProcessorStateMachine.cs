using Automatonymous;
using Automatonymous.Binders;
using Processing.App.Publishers;
using Processing.Models;
using System;
using System.Threading;

namespace Processing.App
{
    public class OrderProcessorStateMachine : MassTransitStateMachine<OrderProcessingState>
    {
        private IOrderPublisher OrderPublisher { get; }

        public OrderProcessorStateMachine(IOrderPublisher orderPublisher)
        {
            OrderPublisher = orderPublisher;

            InstanceState(x => x.State);

            ConfigureCorrelationIds();
            Initially(SetOrderSummitedHandler());

            During(Processing, SetStockReservedHandler(), SetPaymentProcessedHandler());

            During(Processing,
                When(ReadyForShipment)
                    .Then(c => Console.WriteLine($"Order {c.Instance.OrderId} was allowed to be shipped"))
                    .Then(c => Thread.Sleep(2000))
                    .ThenAsync(c => OrderPublisher.OrderApproved(new OrderApprovedModel
                    {
                        OrderId = c.Instance.OrderId
                    }))
                    .Finalize());

            CompositeEvent(() => ReadyForShipment, order => order.CompositeStatus, PaymentProcessed, StockReserved);
        }

        private void ConfigureCorrelationIds()
        {
            Event(() => OrderSubmitted, x => x.CorrelateById(c => c.Message.CorrelationId).SelectId(c => c.Message.CorrelationId));
            Event(() => PaymentProcessed, x => x.CorrelateById(c => c.Message.CorrelationId));
            Event(() => StockReserved, x => x.CorrelateById(c => c.Message.CorrelationId));
        }


        private EventActivityBinder<OrderProcessingState, Orders.Models.INewOrderModel> SetOrderSummitedHandler() =>
            When(OrderSubmitted).Then(c =>
                {
                    c.Instance.OrderSubmitted = DateTimeOffset.Now;
                    c.Instance.OrderId = c.Data.OrderId;
                    Console.WriteLine($"New Order {c.Data.OrderId} received. CorrelationId: {c.Data.CorrelationId}");
                })
                .TransitionTo(Processing);


        private EventActivityBinder<OrderProcessingState, Finances.Models.IOrderPaidModel> SetPaymentProcessedHandler() =>
            When(PaymentProcessed)
                .Then(c =>
                {
                    c.Instance.PaymentProcessed = DateTimeOffset.Now;
                    Console.WriteLine($"Payment for Order {c.Data.OrderId} was received. CorrelationId: {c.Data.CorrelationId}");
                });


        private EventActivityBinder<OrderProcessingState, Warehouses.Models.IStockExistModel> SetStockReservedHandler() =>
            When(StockReserved).Then(c =>
                {
                    c.Instance.StockReserved = DateTimeOffset.Now;
                    Console.WriteLine($"Stock reserved for Order {c.Data.OrderId}. CorrelationId: {c.Data.CorrelationId}");
                })
                .Then(c => Console.WriteLine($"Payment processed to {c.Data.CorrelationId} received"));


        public State Processing { get; set; }
        public Event ReadyForShipment { get; set; }
        public Event<Orders.Models.INewOrderModel> OrderSubmitted { get; set; }
        public Event<Finances.Models.IOrderPaidModel> PaymentProcessed { get; set; }
        public Event<Warehouses.Models.IStockExistModel> StockReserved { get; set; }
    }

}
