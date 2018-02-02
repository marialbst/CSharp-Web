namespace GameStore.App.Controllers
{
    using System;
    using System.Linq;
    using SimpleMvc.Framework.Contracts;

    public class AdminController : BaseController
    {
        public IActionResult AddGame()
        {
            if (!this.User.IsAuthenticated)
            {
                return this.RedirectToAction(LoginPath);
            }

            if (!this.UserIsAdmin)
            {
                return this.RedirectToAction(IndexPath);
            }

            return View();
        }

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

            var gameList = this.GameService
                .All()
                .Select(g => $@"
                    <tr>
                        <td>{g.Id}</td>
                        <td>{g.Title}</td>
                        <td>{g.Size} GB</td>
                        <td>{g.Price} &euro;</td>
                        <td>
                            <a href=""/admin/editgame?id={g.Id}"" class=""btn btn-warning btn-sm"">Edit</a>
                            <a href=""/admin/deletegame?id={g.Id}"" class=""btn btn-danger btn-sm"">Delete</a>
                        </td>
                    </tr>
                ");

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

            this.ViewModel["games"] = string.Join(Environment.NewLine, gameList);
            
            return this.View();
        }
    }
}
