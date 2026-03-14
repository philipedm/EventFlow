using Contract;

namespace ApiProducer1.Interfaces
{
    public interface IOrderService
    {
        Task<Guid> CreateOrderAsync(OrderCreated order);
    }
}
