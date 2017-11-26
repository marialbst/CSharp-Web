namespace WebServer.ByTheCake.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;
    using ViewModels.Products;

    public class ProductService : IProductService
    {
        public void Create(string name, decimal price, string imageUrl)
        {
            using (var db = new ByTheCakeDbContext())
            {
                Product product = new Product()
                {
                    Name = name,
                    Price = price,
                    ImageUrl = imageUrl
                };

                db.Add(product);
                db.SaveChanges();
            }
        }

        public IEnumerable<ProductsListViewModel> All(string searchTerm = null)
        {
            using (var db = new ByTheCakeDbContext())
            {
                var products = db.Products.AsQueryable();

                if(!string.IsNullOrEmpty(searchTerm))
                {
                    products = products.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()));
                }

                return products
                    .Select(p => new ProductsListViewModel()
                    {
                        Name = p.Name,
                        Price = p.Price,
                        Id = p.Id
                    }).ToList();
            }
        }

        public ProductDetailsViewModel Find(int id)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db.Products
                    .Where(p => p.Id == id)
                    .Select(p => new ProductDetailsViewModel()
                    {
                        Name = p.Name,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl
                    })
                    .FirstOrDefault();
            }
        }

        public bool Exists(int id)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db.Products.Any(p => p.Id == id);
            }
        }

        public IEnumerable<ProductCartViewModel> CartProducts(ICollection<int> ids)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db.Products
                    .Where(p => ids.Contains(p.Id))
                    .Select(p => new ProductCartViewModel()
                    {
                        Name = p.Name,
                        Price = p.Price
                    }).ToList();
            }
        }

        public List<Product> GetProductsById(ICollection<int> productIds)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db.Products.Where(p => productIds.Contains(p.Id)).ToList();
            }
        }
    }
}
