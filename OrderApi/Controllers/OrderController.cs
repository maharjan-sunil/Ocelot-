using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models;
using OrderAPI.Services;

namespace OrderAPI.Controllers
{
   // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        public OrderController(IOrderService _orderService)
        {
            orderService = _orderService;
        }

        [HttpGet]
        public IActionResult OrderList()
        {
            var orderList = orderService.GetOrderList();
            if (orderList == null) return NotFound("Unable to retrive orders");

            return Ok(orderList);
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var order = orderService.GetOrderById(id);
            if (order == null) return NotFound($"Unable to find a order with Id: {id}");

            return Ok(order);
        }

        [HttpPost]
        public IActionResult AddOrder(Order order)
        {
            var result = orderService.AddOrder(order);
            if (result == null) return BadRequest($"Unable to add a order with data: {order}");

            return Ok(result);
        }

        [HttpPut]
        public IActionResult UpdateOrder(Order order)
        {
            var result = orderService.UpdateOrder(order);
            if (result == null) return BadRequest($"Unable to update a order with data: {order}");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var result = orderService.DeleteOrder(id);
            if (result == null) return BadRequest($"Unable to delete a order with Id: {id}");

            return Ok(result);
        }
    }
}
