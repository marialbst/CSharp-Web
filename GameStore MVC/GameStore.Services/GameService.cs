namespace GameStore.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using GameStore.Data;
    using GameStore.Services.Models;

    public class GameService : IGameService
    {
        public IEnumerable<AllGamesModel> All()
        {
            using (var db = new GameStoreMvcDbContext())
            {
                return db.Games
                    .ToList()
                    .Select(g => new AllGamesModel
                    {
                        Id = g.Id,
                        Title = g.Title,
                        Size = g.Size,
                        Price = g.Price
                    })
                    .ToList();
            }
        }
    }
}
