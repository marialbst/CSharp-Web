namespace WebServer.ByTheCake.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;
    using WebServer.ByTheCake.ViewModels.Products;

    public class OrderService : IOrderService
    {
        public void CreateOrder(int userId, IEnumerable<int> productIds)
        {
            using (var db = new ByTheCakeDbContext())
            {
                Order order = new Order()
                {
                    UserId = userId,
                    CreationTime = DateTime.UtcNow,
                    Products = productIds.Select(id => new OrderProduct()
                    {
                        ProductId = id
                    })
                    .ToList()
                };

                db.Add(order);
                db.SaveChanges();
            }
        }

        public IEnumerable<OrderListViewModel> GetOrdersByUser(int userId)
        {
            using (var db = new ByTheCakeDbContext())
            {
                List<Order> ordersByUser = db.Orders.Where(o => o.UserId == userId).ToList();

                if (!ordersByUser.Any())
                {
                    return null;
                }

                return ordersByUser
                    .Select(o => new OrderListViewModel()
                    {
                        Id = o.Id,
                        CreationDate = o.CreationTime,
                        Price = o.Products.Select(p => p.Product.Price).Sum()
                    });   
            }
        }
    }
}
