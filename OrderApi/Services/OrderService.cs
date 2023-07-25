using Microsoft.EntityFrameworkCore;
using OrderAPI.Data;
using OrderAPI.Models;
using Serilog;

namespace OrderAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly DbContextClass _dbContext;
        private readonly ILogger<OrderService> _logger;

        public OrderService(DbContextClass dbContext, ILogger<OrderService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public IEnumerable<Order> GetOrderList()
        {
            try
            {
                var orders = _dbContext.Orders.ToList();
                _logger.LogInformation("Retrieved {Count} orders", orders.Count);
                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Unable to retrive orders", ex);
            }

            return null;
        }

        public Order GetOrderById(int id)
        {
            try
            {
                var order = _dbContext.Orders.Where(x => x.OrderId == id).FirstOrDefault();
                if (order == null)
                {
                    _logger.LogInformation("Unable to find a order with Id: {id}", id);
                    return null;
                }

                _logger.LogInformation("Retrieved a order with Id: {id}", id);
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unable to find a order with Id: {id}", ex);
            }

            return null;
        }

        public Order AddOrder(Order order)
        {
            try
            {
                var checkOrderExists = _dbContext.Orders.AsNoTracking().Where(x => x.OrderId == order.OrderId).FirstOrDefault();
                if (checkOrderExists != null)
                {
                    _logger.LogInformation("Order with Id: {Id} already exists", order.OrderId);
                    return null;
                }

                var result = _dbContext.Orders.Add(order);
                _dbContext.SaveChanges();
                _logger.LogInformation("Successfully added the order with data: {order}", order);

                return result.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unable to add a order with data: {order}", ex);
            }

            return null;
        }

        public Order UpdateOrder(Order order)
        {
            try
            {
                var checkOrderExists = _dbContext.Orders.AsNoTracking().Where(x => x.OrderId == order.OrderId).FirstOrDefault();
                if (checkOrderExists == null)
                {
                    _logger.LogInformation("Order with Id: {Id} doesn't exists", order.OrderId);
                    return null;
                }

                var result = _dbContext.Orders.Update(order);
                _dbContext.SaveChanges();
                _logger.LogInformation("Successfully updated the order with data: {order}", order);

                return result.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unable to update a order with data: {order}", ex);
            }

            return null;
        }

        public bool? DeleteOrder(int id)
        {
            try
            {
                var checkOrderExists = _dbContext.Orders.AsNoTracking().Where(x => x.OrderId == id).FirstOrDefault();
                if (checkOrderExists == null)
                {
                    _logger.LogInformation("Order with Id: {id} doesn't exists", id);
                    return null;
                }

                var filteredData = _dbContext.Orders.Where(x => x.OrderId == id).FirstOrDefault();
                var result = _dbContext.Remove(filteredData);
                _dbContext.SaveChanges();

                if (result != null)
                    _logger.LogInformation("Successfully deleted the order with Id: {id}", id);
                else
                    _logger.LogInformation("Unable to delete a order with Id: {id}", id);

                return result != null ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unable to delete a order with Id: {id}", ex);
            }

            return null;
        }
    }
}
