namespace WebServer.ByTheCake.Controllers
{
    using ViewModels;
    using ViewModels.Products;
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;
    using System;
    using System.Linq;
    using Services;
    using Services.Contracts;
    using Server.Enums;
    using WebServer.ByTheCake.Views;
    using WebServer.Server.HTTP;

    public class ProductsController : Controller
    {
        private const string AddProductPath = @"Products\add";
        private const string SearchProductPath = @"Products\search";
        private const string CartPath = @"Products\cart";
        private const string SuccessPath = @"Products\success";
        private const string OrdersPath = @"Products\orders";

        private readonly IProductService productService;
        private readonly IOrderService orderService;
        private readonly IUserService userService;

        public ProductsController()
        {
            this.productService = new ProductService();
            this.orderService = new OrderService();
            this.userService = new UserService();
        }

        public IHttpResponse Add()
        {
            this.ViewData["showResult"] = "none";
            return this.FileViewResponse(AddProductPath);
        }

        public IHttpResponse Add(AddProductViewModel model)
        {
            if(model.Name.Length < 3 
                || model.ImageUrl.Length < 3 
                || model.Name.Length > 30 
                || model.ImageUrl.Length > 2000)
            {
                this.AddError("Invalid cake data");
                this.ViewData["showResult"] = "none";

                return this.FileViewResponse(AddProductPath);
            }

            this.productService.Create(model.Name, model.Price, model.ImageUrl);
            
            this.ViewData["name"] = model.Name;
            this.ViewData["price"] = $"{model.Price:f2}";
            this.ViewData["url"] = model.ImageUrl;
            this.ViewData["showResult"] = "block";

            return this.FileViewResponse(AddProductPath);
        }

        public IHttpResponse Search(IHttpRequest req)
        {
            const string searchKey = "name";
            var queryParameters = req.QueryParameters;

            this.ViewData["result"] = string.Empty;
            this.ViewData["name"] = string.Empty;

            var searchTerm = queryParameters.ContainsKey(searchKey)
                ? queryParameters[searchKey]
                : null;

            var products = this.productService.All(searchTerm);

            if(!products.Any())
            {
                this.ViewData["result"] = "No cakes found";
            }
            else
            {
                var foundProducts = products
                    .Select(c => $"<div><a href=\"/products/{c.Id}\">{c.Name}</a> ${c.Price:f2}<a href=\"/order?id={c.Id}&returnUrl={searchTerm}\"><button>Order</button></a></div>");

                this.ViewData["result"] = string.Join(Environment.NewLine, foundProducts);
            }

            var shoppingCart = req.Session.Get<Cart>(Cart.SessionKey);

            var totalProducts = shoppingCart.ProductIds.Count;
            var totalProductsText = totalProducts != 1 ? "products" : "product";

            this.ViewData["products"] = $"{totalProducts} {totalProductsText}";

            return this.FileViewResponse(SearchProductPath);
        }

        public IHttpResponse Details(int id)
        {
            var product = this.productService.Find(id);

            if(product == null)
            {
                return new ViewResponse(HttpStatusCode.NotFound, new NotFoundView());
            }

            this.ViewData["name"] = product.Name;
            this.ViewData["price"] = product.Price.ToString("f2");
            this.ViewData["url"] = product.ImageUrl;

            return this.FileViewResponse(@"Products\details");
        }

        public IHttpResponse Order(IHttpRequest request)
        {
            var id = int.Parse(request.QueryParameters["id"]);

            //get form db
            var isExisting = this.productService.Exists(id);

            var cart = request.Session.Get<Cart>(Cart.SessionKey);

            if (!isExisting)
            {
                return new ViewResponse(HttpStatusCode.NotFound, new NotFoundView());
            }

            cart.ProductIds.Add(id);

            string returnPath = "/search";

            string returnUrl = request.QueryParameters["returnUrl"];

            if (!string.IsNullOrEmpty(returnUrl))
            {
                returnPath = $"{returnPath}?name={returnUrl}";
            }

            return new RedirectResponse(returnPath);
        }

        public IHttpResponse ShowCart(IHttpRequest req)
        {
            var cart = req.Session.Get<Cart>(Cart.SessionKey);

            if (!cart.ProductIds.Any())
            {
                this.ViewData["products"] = "No items in your cart";
                this.ViewData["price"] = "$0.00";
            }
            else
            {
                var products = this.productService
                    .CartProducts(cart.ProductIds);

                var productDivs = products
                    .Select(c => $"<div>{c.Name} - ${c.Price:f2}</div><br />");

                var price = products
                    .Select(p => p.Price)
                    .Sum();

                this.ViewData["products"] = string.Join(Environment.NewLine, productDivs);
                this.ViewData["price"] = $"${price:f2}";
            }

            return this.FileViewResponse(CartPath);
        }

        public IHttpResponse Success(IHttpRequest req)
        {

            var cart = req.Session.Get<Cart>(Cart.SessionKey);

            if (cart.ProductIds.Count > 0)
            {
                int userId = this.ValidateUser(req);
                
                this.orderService.CreateOrder(userId, cart.ProductIds);

                cart.ProductIds.Clear();
                this.ViewData["message"] = "Successfully finished your order!";
            }
            else
            {
                this.ViewData["message"] = "No cakes in the cart!";
            }

            return this.FileViewResponse(SuccessPath);
        }

        public IHttpResponse AllOrders(IHttpRequest req)
        {
            int userId = this.ValidateUser(req);

            var ordersByUser = this.orderService.GetOrdersByUser(userId);

            if (ordersByUser == null)
            {
                this.ViewData["showResult"] = "none";
                this.ViewData["showMessage"] = "block";
                this.ViewData["message"] = "You don't have any orders yet";
                this.ViewData["result"] = string.Empty;
                return this.FileViewResponse(OrdersPath);
            }

            var rows = ordersByUser
                .Select(o => 
                $"<tr>" +
                $"  <td>" +
                $"      <a href=\"/orders/{o.Id}\">{o.Id}</a>" +
                $"  </td>"+
                $"  <td>{o.CreationDate.ToShortDateString()}</td>"+
                $"  <td>${o.Price:f2}</td>" +
                $"</tr>"
                );

            string result = string.Join(Environment.NewLine, rows);

            this.ViewData["result"] = result;
            this.ViewData["showMessage"] = "none";
            return this.FileViewResponse(OrdersPath);
        }

        private int ValidateUser(IHttpRequest req)
        {
            var currentUsername = req.Session.Get<string>(SessionStore.CurrentUserKey);

            var id = this.userService.GetUserId(currentUsername);

            if (id == null)
            {
                throw new InvalidOperationException($"User {currentUsername} does not exist");
            }

            int userId = id.Value;
            return userId;
        }
    }
}