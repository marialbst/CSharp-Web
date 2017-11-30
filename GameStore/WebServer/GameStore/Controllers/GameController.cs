namespace WebServer.GameStore.Controllers
{
    using Server.HTTP.Contracts;
    using Service;
    using Service.Contracts;
    using Server.HTTP.Response;
    using System;

    public class GameController : Controller
    {
        private const string DetailsPath = @"Game\details";

        private readonly IGameService gameService;

        public GameController(IHttpRequest request)
            :base(request)
        {
            this.gameService = new GameService();
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

            return this.FileViewResponse(DetailsPath);
        }
    }
}
