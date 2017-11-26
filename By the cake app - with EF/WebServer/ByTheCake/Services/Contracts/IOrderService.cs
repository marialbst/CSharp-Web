namespace WebServer.ByTheCake.Services.Contracts
{
    using System.Collections.Generic;
    using ViewModels.Products;

    public interface IOrderService
    {
        void CreateOrder(int userId, IEnumerable<int> productIds);

        IEnumerable<OrderListViewModel> GetOrdersByUser(int userId);
    }
}
