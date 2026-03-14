using ApiProducer1.Interfaces;
using Contract;
using EventMessaging;
using EventMessaging.Interfaces;

namespace ApiProducer1.Services
{
    public class OrderService : IOrderService
    {
        private readonly IEventPublisher _publisher;

        public OrderService(IEventPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task<Guid> CreateOrderAsync(OrderCreated order)
        {
            order.Id = Guid.NewGuid();

            var envelope = MessageEnvelope.FromJson(order, subject: "OrderCreated",
                    messageId: order.Id.ToString(),
                    correlationId: order.Id.ToString(),
                    props: new Dictionary<string, object> { ["product"] = order.Product, ["price"] = order.Price, ["eventType"] = "OrderCreated" });

            await _publisher.PublishAsync(topicName: "orders", message: envelope);

            return order.Id;
        }
    }
}
