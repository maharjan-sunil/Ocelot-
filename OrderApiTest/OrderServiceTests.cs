using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using OrderAPI.Data;
using OrderAPI.Models;
using OrderAPI.Services;

namespace OrderApiTest
{
    public class OrderServiceTests
    {
        private readonly DbContextOptions<DbContextClass> options = new DbContextOptionsBuilder<DbContextClass>()
            .UseInMemoryDatabase(databaseName: "ecommerce-app")
            .Options;
        private readonly Mock<ILogger<OrderService>> _loggerMock = new Mock<ILogger<OrderService>>();

        public OrderServiceTests()
        {

        }

        [Fact]
        public void GetOrderList_ShouldReturnListOfOrder_WhenOrderExists()
        {
            //Arrange
            IEnumerable<Order> orders = Enumerable.Empty<Order>();
            using (var context = new DbContextClass(options, true))
            {
                context.Orders.Add(new Order
                {
                    OrderId = 1,
                    OrderName = "OrderName",
                    ProductId = 1,
                    CustomerName = "CutomerName",
                    CustomerAddress = "CustomerAddress",
                    Quantity = 1,
                    OrderDate = DateTime.Now,
                });
                context.Orders.Add(new Order
                {
                    OrderId = 2,
                    OrderName = "OrderName2",
                    ProductId = 1,
                    CustomerName = "CutomerName",
                    CustomerAddress = "CustomerAddress",
                    Quantity = 1,
                    OrderDate = DateTime.Now,
                });
                context.SaveChanges();
            }

            //Act
            using (var context = new DbContextClass(options, true))
            {
                IOrderService _orderService = new OrderService(context, _loggerMock.Object);
                orders = _orderService.GetOrderList();
            }

            //Assert
            Assert.Equal(orders.Count(), 2);
        }

        [Fact]
        public void GetOrderById_ShouldReturnOrder_WhenOrderExists()
        {
            //Arrange
            var orderId = 3;
            Order order = new Order();
            using (var context = new DbContextClass(options, true))
            {
                context.Orders.Add(new Order
                {
                    OrderId = orderId,
                    OrderName = "OrderName3",
                    ProductId = 1,
                    CustomerName = "CutomerName",
                    CustomerAddress = "CustomerAddress",
                    Quantity = 1,
                    OrderDate = DateTime.Now,
                });
                context.SaveChanges();
            }

            //Act
            using (var context = new DbContextClass(options, true))
            {
                IOrderService _orderService = new OrderService(context, _loggerMock.Object);
                order = _orderService.GetOrderById(orderId);
            }

            //Assert
            Assert.Equal(orderId, order.OrderId);
        }

        [Fact]
        public void GetOrderById_ShouldReturnNothing_WhenOrderDoesNotExists()
        {
            //Arrange
            Order order = new Order();

            //Act
            using (var context = new DbContextClass(options, true))
            {
                IOrderService _orderService = new OrderService(context, _loggerMock.Object);
                order = _orderService.GetOrderById(It.IsAny<int>());
            }

            //Assert
            Assert.Null(order);
        }

        [Fact]
        public void AddOrder_ShouldReturnOrder_WhenOrderInsertedSuccessfully()
        {
            //Arrange
            Order order = new Order();

            Order orderItem = new Order 
            {
                OrderName = "OrderName100",
                ProductId = 1,
                CustomerName = "CutomerName",
                CustomerAddress = "CustomerAddress",
                Quantity = 1,
                OrderDate = DateTime.Now,
            };

            //Act
            using (var context = new DbContextClass(options, true))
            {
                IOrderService _orderService = new OrderService(context, _loggerMock.Object);
                order = _orderService.AddOrder(orderItem);
            }

            //Assert
            Assert.NotNull(order);
        }

        [Fact]
        public void AddOrder_ShouldReturnNull_WhenDuplicateOrderIdIsPassed()
        {
            //Arrange
            Order order = new Order();
            var orderId = 5;
            using (var context = new DbContextClass(options, true))
            {
                context.Orders.Add(new Order
                {
                    OrderId = orderId,
                    OrderName = "OrderName5",
                    ProductId = 1,
                    CustomerName = "CutomerName",
                    CustomerAddress = "CustomerAddress",
                    Quantity = 1,
                    OrderDate = DateTime.Now,
                });
                context.SaveChanges();
            }

            Order orderItem = new Order
            {
                OrderId = orderId,
                OrderName = "OrderName5",
                ProductId = 1,
                CustomerName = "CutomerName",
                CustomerAddress = "CustomerAddress",
                Quantity = 1,
                OrderDate = DateTime.Now,
            };

            //Act
            using (var context = new DbContextClass(options, true))
            {
                IOrderService _orderService = new OrderService(context, _loggerMock.Object);
                order = _orderService.AddOrder(orderItem);
            }

            //Assert
            Assert.Null(order);
        }

        [Fact]
        public void UpdateOrder_ShouldReturnUpdatedOrder_WhenOrderUpdatedSuccessfully()
        {
            //Arrange
            Order order = new Order();

            var orderId = 6;
            using (var context = new DbContextClass(options, true))
            {
                context.Orders.Add(new Order
                {
                    OrderId = orderId,
                    OrderName = "OrderName6",
                    ProductId = 1,
                    CustomerName = "CutomerName",
                    CustomerAddress = "CustomerAddress",
                    Quantity = 1,
                    OrderDate = DateTime.Now,
                });
                context.SaveChanges();
            }

            Order orderItem = new Order
            {
                OrderId = orderId,
                OrderName = "OrderNameChanged",
                ProductId = 1,
                CustomerName = "CutomerName",
                CustomerAddress = "CustomerAddress",
                Quantity = 1,
                OrderDate = DateTime.Now,
            };

            //Act
            using (var context = new DbContextClass(options, true))
            {
                IOrderService _orderService = new OrderService(context, _loggerMock.Object);
                order = _orderService.UpdateOrder(orderItem);
            }

            //Assert
            Assert.NotNull(order);
            Assert.Equal(orderItem.OrderName, order.OrderName);
        }

        [Fact]
        public void UpdateOrder_ShouldReturnNull_WhenOrderIdDoesnotExists()
        {
            //Arrange
            Order order = new Order();
            var orderId = 10;

            Order orderItem = new Order
            {
                OrderId = orderId,
                OrderName = "OrderName10",
                ProductId = 1,
                CustomerName = "CutomerName",
                CustomerAddress = "CustomerAddress",
                Quantity = 1,
                OrderDate = DateTime.Now,
            };

            //Act
            using (var context = new DbContextClass(options, true))
            {
                IOrderService _orderService = new OrderService(context, _loggerMock.Object);
                order = _orderService.UpdateOrder(orderItem);
            }

            //Assert
            Assert.Null(order);
        }

        [Fact]
        public void DeleteOrder_ShouldReturnTrue_WhenOrderDeletedSuccessfully()
        {
            //Arrange
            bool? result = null;

            var orderId = 7;
            using (var context = new DbContextClass(options, true))
            {
                context.Orders.Add(new Order
                {
                    OrderId = orderId,
                    OrderName = "OrderName7",
                    ProductId = 1,
                    CustomerName = "CutomerName",
                    CustomerAddress = "CustomerAddress",
                    Quantity = 1,
                    OrderDate = DateTime.Now,
                });
                context.SaveChanges();
            }

            //Act
            using (var context = new DbContextClass(options, true))
            {
                IOrderService _orderService = new OrderService(context, _loggerMock.Object);
                result = _orderService.DeleteOrder(orderId);
            }

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void DeleteOrder_ShouldReturnNull_WhenOrderIdDoesnotExists()
        {
            //Arrange
            bool? result = null;
            var orderId = 10;

            //Act
            using (var context = new DbContextClass(options, true))
            {
                IOrderService _orderService = new OrderService(context, _loggerMock.Object);
                result = _orderService.DeleteOrder(orderId);
            }

            //Assert
            Assert.Null(result);
        }
    }
}