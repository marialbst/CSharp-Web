namespace WebServer.ByTheCake.Controllers
{
    using Models;
    using Views;
    using Server.Enums;
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;
    using System.Collections.Generic;

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

        public IHttpResponse Search()
        {
            return new ViewResponse(HttpStatusCode.Ok, new SearchCakeView());
        }

        public IHttpResponse Search(Dictionary<string, string> queryPar)
        {
            if (!queryPar.ContainsKey("name"))
            {
                return new ViewResponse(HttpStatusCode.Ok, new SearchCakeView());
            }

            if (!string.IsNullOrWhiteSpace(queryPar["name"]))
            {
                return new ViewResponse(HttpStatusCode.Ok, new SearchCakeView(CakeList.SearchCakes(queryPar["name"])));
            }

            return new ViewResponse(HttpStatusCode.Ok, new SearchCakeView("error"));
        }
    }
}
