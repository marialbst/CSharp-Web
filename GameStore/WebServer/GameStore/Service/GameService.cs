namespace WebServer.GameStore.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using ViewModels.Game;
    using Data.Models;

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

        public void Create(string title, string trailerId, string imageUrl, double size, decimal price, string description, DateTime releaseDate)
        {
            using (var db = new GameStoreDbContext())
            {
                var game = new Game()
                {
                    Title = title,
                    TrailerId = trailerId,
                    ImageThumbnail = imageUrl,
                    Size = size,
                    Price = price,
                    Description = description,
                    ReleaseDate = releaseDate
                };

                db.Add(game);
                db.SaveChanges();
            }
        }

        public AddGameViewModel Find(int id)
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Games
                    .Where(g => g.Id == id)
                    .Select(g => new AddGameViewModel()
                    {
                        Title = g.Title,
                        Description = g.Description,
                        TrailerId = g.TrailerId,
                        ImageThumbnail = g.ImageThumbnail,
                        Size = g.Size,
                        Price = g.Price,
                        ReleaseDate = g.ReleaseDate

                    })
                    .SingleOrDefault();
            }
        }

        public bool Edit(int id, string title, string trailerId, string imageUrl, double size, decimal price, string description, DateTime releaseDate)
        {
            using (var db = new GameStoreDbContext())
            {
                var game = db.Games.FirstOrDefault(g => g.Id == id);

                if(game == null)
                {
                    return false;
                }

                var changesMade = false;

                if(game.Title != title 
                   || game.TrailerId != trailerId
                   || game.ImageThumbnail != imageUrl
                   || game.Size != size
                   || game.Price != price
                   || game.Description != description
                   || game.ReleaseDate != releaseDate)
                {
                    game.Title = title;
                    game.ImageThumbnail = imageUrl;
                    game.TrailerId = trailerId;
                    game.Size = size;
                    game.Price = price;
                    game.Description = description;
                    game.ReleaseDate = releaseDate;

                    changesMade = true;

                    db.SaveChanges();
                }

                return changesMade;
            }
        }

        public void Delete(int id)
        {
            using(var db = new GameStoreDbContext())
            {
                var game = db.Games.FirstOrDefault(g => g.Id == id);
                
                db.Games.Remove(game);
                db.SaveChanges();
            }
        }

        public bool Exists(int id)
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Games.Any(g => g.Id == id);
            }
        }

        public IList<IndexViewGame> All()
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Games.Select(g => new IndexViewGame()
                {
                    Id = g.Id,
                    Title = g.Title,
                    ImageThumbnail = g.ImageThumbnail,
                    Description = g.Description,
                    Price = g.Price,
                    Size = g.Size
                }).ToList();
            }
        }

        public IList<IndexViewGame> All(int id)
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Games
                    .Where(g => g.Users.Select(u => u.UserId)
                    .Contains(id))
                    .Select(g => new IndexViewGame()
                    {
                        Id = g.Id,
                        Title = g.Title,
                        ImageThumbnail = g.ImageThumbnail,
                        Description = g.Description,
                        Price = g.Price,
                        Size = g.Size
                    }).ToList();
            }
        }
    }
}
