using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechnicalAssignment.Models;
using TechnicalAssignment.Repositories;

namespace TechnicalAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderRepository _orderRepository;

        public OrdersController(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        [HttpGet]
        public ActionResult<IEnumerable<OrderDto>> GetOrders()
        {
            var orders = _orderRepository.GetOrders();
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public ActionResult<OrderDto> GetOrderById(int id)
        {
            var order = _orderRepository.GetOrderById(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        [HttpPost]
        public ActionResult<long> CreateOrder(CreateOrderDto orderDto)
        {
            var orderId = _orderRepository.AddOrder(orderDto);

            return CreatedAtAction(nameof(GetOrderById), new { id = orderId }, orderId);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, CreateOrderDto order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _orderRepository.UpdateOrder(order);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            _orderRepository.DeleteOrder(id);

            return NoContent();
        }

    }
}
