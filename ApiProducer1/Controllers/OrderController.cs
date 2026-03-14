using ApiProducer1.Interfaces;
using Contract;
using Microsoft.AspNetCore.Mvc;

namespace ApiProducer1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public IActionResult CreateOrder(OrderCreated order)
        {
            try
            {
                var idOrderCreated = _orderService.CreateOrderAsync(order);
                return Ok(new { idOrderCreated.Result });
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create order: {ex.Message}");
            }
        }
    }
}
