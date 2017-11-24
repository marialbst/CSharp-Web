namespace WebServer.ByTheCake.Controllers
{
    using ViewModels;
    using ViewModels.Products;
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;
    using System;
    using System.Linq;

    public class ProductsController : Controller
    {
        private readonly CakeList cakeList;

        public ProductsController()
        {
            this.cakeList = new CakeList();
        }

        public IHttpResponse Add()
        {
            this.ViewData["showResult"] = "none";
            return this.FileViewResponse(@"Products\add");
        }

        public IHttpResponse Add(AddProductViewModel model)
        {
            if(model.Name.Length < 3 
                || model.ImageUrl.Length < 3 
                || model.Name.Length > 30 
                || model.ImageUrl.Length > 2000)
            {
                this.ViewData["showError"] = "block red";
                this.ViewData["error"] = "Invaid cake data";

                return this.FileViewResponse(@"Products\add");
            }
            //this.cakeList.Add(name, price);

            //this.ViewData["name"] = name;
            //this.ViewData["price"] = price;
            //this.ViewData["showResult"] = "block";

            return this.FileViewResponse(@"Products\add");
        }

        public IHttpResponse Search(IHttpRequest req)
        {
            const string searchKey = "name";
            var queryParameters = req.QueryParameters;

            this.ViewData["result"] = string.Empty;
            this.ViewData["name"] = string.Empty;

            if (queryParameters.ContainsKey(searchKey))
            {
                string search = queryParameters[searchKey];
                this.ViewData["name"] = search;

                var foundCakes = this.cakeList
                    .AllCakes()
                    .Where(c => c.Name.ToLower().Contains(search.ToLower()))
                    .Select(c => $"<div>{c.Name} ${c.Price:f2}<a href=\"/order?id={c.Id}&returnUrl={search}\"><button>Order</button></a></div>");

                var result = string.Empty;

                if (foundCakes.Any())
                {
                    result = string.Join(Environment.NewLine, foundCakes);
                }
                else
                {
                    result = "No cakes found";
                }

                this.ViewData["result"] = result;
            }
            else
            {
                this.ViewData["result"] = "Please, enter search term";
            }

            var shoppingCart = req.Session.Get<Cart>(Cart.SessionKey);

            var totalProducts = shoppingCart.Cakes.Count;
            var totalProductsText = totalProducts != 1 ? "products" : "product";

            this.ViewData["products"] = $"{totalProducts} {totalProductsText}";

            return this.FileViewResponse(@"Products\search");
        }

        public IHttpResponse Order(IHttpRequest request)
        {
            var id = int.Parse(request.QueryParameters["id"]);

            var cake = this.cakeList.GetCakeById(id);

            var cart = request.Session.Get<Cart>(Cart.SessionKey);

            if (cake != null)
            {
                cart.Cakes.Add(cake);
            }

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

            if (!cart.Cakes.Any())
            {
                this.ViewData["products"] = "No items in your cart";
                this.ViewData["price"] = "$0.00";
            }
            else
            {
                var products = cart.Cakes.Select(c => $"<div>{c.Name} - ${c.Price:f2}</div><br />");
                var price = cart.TotalCost();

                this.ViewData["products"] = string.Join(Environment.NewLine, products);
                this.ViewData["price"] = $"${price:f2}";
            }

            return this.FileViewResponse(@"Products\cart");
        }

        public IHttpResponse Success(IHttpRequest req)
        {

            var cart = req.Session.Get<Cart>(Cart.SessionKey);

            if (cart.Cakes.Count > 0)
            {
                cart.Cakes.Clear();
                this.ViewData["message"] = "Successfully finished your order!";
            }
            else
            {
                this.ViewData["message"] = "No cakes in the cart!";
            }

            return this.FileViewResponse(@"Products\success");
        }
    }
}