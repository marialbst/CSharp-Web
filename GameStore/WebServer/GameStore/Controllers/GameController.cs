namespace WebServer.GameStore.Controllers
{
    using Server.HTTP.Contracts;
    using Service;
    using Service.Contracts;
    using Server.HTTP.Response;
    using System;
    using WebServer.Server.HTTP;
    using WebServer.GameStore.ViewModels;
    using System.Collections.Generic;
    using WebServer.GameStore.ViewModels.Game;
    using System.Text;
    using System.Linq;
    using Server.Enums;
    using Server;
    using Data.Models;

    public class GameController : Controller
    {
        private const string DetailsPath = @"Game\details";
        private const string CartPath = @"Game\cart";

        private readonly IGameService gameService;
        private readonly IUserService userService;

        public GameController(IHttpRequest request)
            :base(request)
        {
            this.gameService = new GameService();
            this.userService = new UserService();
        }

        public IHttpResponse Details(int id)
        {
            var model = this.gameService.Find(id);

            if(model == null)
            {
                return new RedirectResponse("/");
            }

            this.ViewData["youtube-id"] = model.TrailerId;
            this.ViewData["title"] = model.Title;
            this.ViewData["description"] = model.Description;
            this.ViewData["releaseDate"] = String.Format("{0:dd/MM/yyyy}", model.ReleaseDate);
            this.ViewData["price"] = model.Price.ToString();
            this.ViewData["size"] = model.Size.ToString();
            this.ViewData["id"] = id.ToString();

            if (this.Request.Session.Get<Cart>(SessionStore.ShoppingCartKey).Games.Select(g => g.Id).Contains(id))
            {
                this.ViewData["hideModal"] = Controller.RemoveHideElementClass;
                this.ViewData["buyButton"] = $"<button class=\"card-button btn btn-primary\" name=\"buy\" data-toggle=\"modal\" data-target=\"#myModal\">Buy</button>";
            }
            else
            {
                this.ViewData["hideModal"] = Controller.AddHideElementClass;
                this.ViewData["buyButton"] = $"<a class=\"btn btn-primary\" href=\"/cart/add/{id}\">Buy</a>";
            }

            return this.FileViewResponse(DetailsPath);
        }

        public IHttpResponse Cart()
        {
            Cart cart = this.Request.Session.Get<Cart>(SessionStore.ShoppingCartKey);

            if(cart == null)
            {
                return new RedirectResponse("/");
            }

            if(cart.Games.Count == 0)
            {
                this.ViewData["showCart"] = Controller.AddHideElementClass;
                this.ViewData["showMessage"] = Controller.RemoveHideElementClass;
                this.ViewData["cart"] = Controller.AddHideElementClass;
                this.ViewData["message"] = "Your cart is empty";

                return this.FileViewResponse(CartPath);
            }

            this.ViewData["showCart"] = Controller.RemoveHideElementClass;
            this.ViewData["showMessage"] = Controller.AddHideElementClass;
            this.ViewData["message"] = Controller.AddHideElementClass;
            this.ViewData["cart"] = this.RenderHtml(cart.Games);

            return this.FileViewResponse(CartPath);
        }

        //get?
        public IHttpResponse CartAdd(int id)
        {
            if (!this.gameService.Exists(id))
            {
                return new ViewResponse(HttpStatusCode.NotFound, new NotFoundView());
            }

            Cart cart = this.Request.Session.Get<Cart>(SessionStore.ShoppingCartKey);

            if (cart == null || cart.Games.Any(g => g.Id == id))
            {
                return new RedirectResponse("/");
            }

            var game = this.gameService.Get(id);
            
            cart.Games.Add(game);

            return new RedirectResponse("/");
        }

        //get?
        public IHttpResponse CartRemove(int id)
        {
            if (!this.gameService.Exists(id))
            {
                return new ViewResponse(HttpStatusCode.NotFound, new NotFoundView());
            }

            Cart cart = this.Request.Session.Get<Cart>(SessionStore.ShoppingCartKey);

            if (cart == null)
            {
                return new RedirectResponse("/");
            }

            if(!cart.Games.Any(g => g.Id == id))
            {
                return new RedirectResponse("/cart");
            }

            var game = cart.Games.First(g => g.Id == id);

            cart.Games.Remove(game);

            return new RedirectResponse("/cart"); 
        }

        public IHttpResponse Order()
        {
            if(!this.Request.Session.Contains(SessionStore.CurrentUserKey))
            {
                return new RedirectResponse("/login");
            }

            string currentUsername = this.Request.Session.Get<string>(SessionStore.CurrentUserKey);

            Cart cart = this.Request.Session.Get<Cart>(SessionStore.ShoppingCartKey);

            if (cart == null || !this.userService.Exists(currentUsername))
            {
                return new RedirectResponse("/");
            }

            var orderGames = cart.Games;
            this.userService.Order(orderGames, currentUsername);
            
            this.Request.Session.Clear(SessionStore.ShoppingCartKey);
            return new RedirectResponse("/");
        }

        private string RenderHtml(HashSet<IndexViewGame> games)
        {
            var result = new StringBuilder();
            result.AppendLine("<div class=\"list-group\">");

            foreach (var game in games)
            {
                result.AppendLine("<div class=\"list-group-item\">");
                result.AppendLine("<div class=\"media\">");
                result.AppendLine($"<a class=\"btn btn-outline-danger btn-lg align-self-center mr-3\" href=\"/cart/remove/{game.Id}\">X</a>");
                result.AppendLine($"<img class=\"d-flex mr-4 align-self-center img-thumbnail\" height=\"127\" src=\"{game.ImageThumbnail}\" width=\"227\" alt=\"image\" />");

                result.AppendLine("<div class=\"media-body align-self-center\">");
                result.AppendLine($"<a href=\"/games/details/{game.Id}\">");
                result.AppendLine($"<h4 class=\"mb-1 list-group-item-heading\">{game.Title}</h4></a>");
                result.AppendLine($"<p>{game.Description.Substring(0,150)}</p>");
                result.AppendLine("</div>");

                result.AppendLine("<div class=\"col-md-2 text-center align-self-center mr-auto\">");
                result.AppendLine($"<h2> {game.Price} &euro; </h2>");
                result.AppendLine("</div>");

                result.AppendLine("</div>");
                result.AppendLine("</div>");
            }

            result.AppendLine("</div>");
            result.AppendLine("<div class=\"col-md-8 ml-auto mr-auto mt-5 text-center\">");
            result.AppendLine($"<h1><strong>Total Price - </strong> {games.Sum(g => g.Price)} &euro;</h1>");
            result.AppendLine("</div>");

            return result.ToString();
        }
    }
}
