namespace GameStore.App.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using GameStore.App.Models.Games;
    using SimpleMvc.Framework.Contracts;

    public class AdminController : BaseController
    {
        public IActionResult Games()
        {
            if (!this.User.IsAuthenticated)
            {
                return this.RedirectToAction(LoginPath);
            }

            if (!this.UserIsAdmin)
            {
                return this.RedirectToAction(IndexPath);
            }

            var gameList = this.GameService.All();

            ////little unnecessary, made just to avoid circular dependancy
            ////and to stick to the convention of our MVC
            //var games = new List<GameListModel>
            //    (
            //        list.Select(g => new GameListModel
            //        {
            //            Id = g.Id,
            //            Title = g.Title,
            //            Size = g.Size,
            //            Price = g.Price
            //        })
            //    );

            return this.View();
        }
    }
}
