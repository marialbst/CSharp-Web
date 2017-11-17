namespace WebServer.ByTheCake.Controllers
{
    using Models;
    using Views;
    using Server.Enums;
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;
    using System.Collections.Generic;
    using System;

    public class CakesController
    {
        public IHttpResponse Add()
        {
            return new ViewResponse(HttpStatusCode.Ok, new AddCakeView());
        }

        public IHttpResponse Add(Dictionary<string, string> formData)
        {
            if (!formData.ContainsKey("name") || !formData.ContainsKey("price"))
            {
                return new ViewResponse(HttpStatusCode.Ok, new AddCakeView("error"));
            }

            if (string.IsNullOrWhiteSpace(formData["name"]) || string.IsNullOrWhiteSpace(formData["price"]))
            {
                return new ViewResponse(HttpStatusCode.Ok, new AddCakeView("error"));
            }

            Cake cake = new Cake(formData["name"], formData["price"]);

            CakeList.SaveCake(cake);

            return new ViewResponse(HttpStatusCode.Ok, new AddCakeView(cake));
        }

        public IHttpResponse Search(IHttpSession session)
        {
            int productsCount = this.GetProductsCount(session);

            return new ViewResponse(HttpStatusCode.Ok, new SearchCakeView(productsCount));
        }

        public IHttpResponse Search(IHttpRequest req)
        {
            Dictionary<string, string> queryPar = req.QueryParameters;
            IHttpSession session = req.Session;

            int productsCount = this.GetProductsCount(session);

            if (!queryPar.ContainsKey("name"))
            {
                return new ViewResponse(HttpStatusCode.Ok, new SearchCakeView(productsCount));
            }

            if (!string.IsNullOrWhiteSpace(queryPar["name"]))
            {
                return new ViewResponse(HttpStatusCode.Ok, new SearchCakeView(productsCount, CakeList.SearchCakes(queryPar["name"]), queryPar["name"]));
            }

            return new ViewResponse(HttpStatusCode.Ok, new SearchCakeView(productsCount,"error"));
        }

        public IHttpResponse Order(IHttpRequest request)
        {
            if (request.QueryParameters.ContainsKey("id"))
            {
                int id = int.Parse(request.QueryParameters["id"]);

                Cake cake = CakeList.GetCakeById(id);
                Cart cart = this.GetCartFromSession(request.Session);             

                if (cake != null)
                {
                    cart.Cakes.Add(cake);
                }
            }

            string returnPath = "/search";

            string returnUrl = request.QueryParameters["returnUrl"];

            if (!string.IsNullOrEmpty(returnUrl))
            {
                returnPath = $"{returnPath}?name={returnUrl}";
            }

            return new RedirectResponse(returnPath);
        }

        public IHttpResponse Cart(IHttpSession session)
        {
            Cart cart = this.GetCartFromSession(session);
            return new ViewResponse(HttpStatusCode.Ok, new CartView(cart));
        }

        public IHttpResponse Cart(IHttpRequest request)
        {
            return new RedirectResponse("/success");
        }

        public IHttpResponse Success(IHttpRequest request)
        {
            Cart cart = this.GetCartFromSession(request.Session);

            if (cart.Cakes.Count > 0)
            {
                cart.Cakes.Clear();
                return new ViewResponse(HttpStatusCode.Ok, new SuccessView("Successfully finished order!"));
            }

            return new ViewResponse(HttpStatusCode.Ok, new SuccessView("You can not finish this order. Your cart is empty!"));
        }

        private int GetProductsCount(IHttpSession session)
        {
            int productsCount = 0;
            
            if (session.Contains("shoppingCart"))
            {
                Cart cart = session.Get<Cart>("shoppingCart");
                productsCount = cart.Cakes.Count;
            }
            return productsCount;
        }

        private Cart GetCartFromSession(IHttpSession session)
        {
            if (!session.Contains("shoppingCart"))
            {
                session.Add("shoppingCart", new Cart());
            }

            return session.Get<Cart>("shoppingCart");
        }
    }
}
