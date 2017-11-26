namespace WebServer.ByTheCake.Services.Contracts
{
    using System.Collections.Generic;
    using WebServer.ByTheCake.Models;
    using WebServer.ByTheCake.ViewModels.Products;

    public interface IProductService
    {
        void Create(string name, decimal price, string imageUrl);

        IEnumerable<ProductsListViewModel> All(string searchTerm = null);

        ProductDetailsViewModel Find(int id);

        bool Exists(int id);

        IEnumerable<ProductCartViewModel> CartProducts(ICollection<int> ids);

        List<Product> GetProductsById(ICollection<int> productIds);
    }
}
