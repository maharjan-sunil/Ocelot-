using OrderAPI.Models;

namespace OrderAPI.Services
{
    public interface IOrderService
    {
        public IEnumerable<Order> GetOrderList();
        public Order GetOrderById(int id);
        public Order AddOrder(Order order);
        public Order UpdateOrder(Order order);
        public bool? DeleteOrder(int id);
    }
}
