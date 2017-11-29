namespace WebServer.GameStore.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using ViewModels.Game;

    public class GameService : IGameService
    {
        public IList<ListGamesViewModel> List()
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Games.Select(g => new ListGamesViewModel()
                {
                    Id = g.Id,
                    Title = g.Title,
                    Price = g.Price,
                    Size = g.Size
                }).ToList();
            }
        }
    }
}
