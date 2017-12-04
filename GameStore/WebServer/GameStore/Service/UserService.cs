namespace WebServer.GameStore.Service
{
    using System.Linq;
    using Contracts;
    using Data;
    using ViewModels.Account;
    using Data.Models;
    using WebServer.GameStore.ViewModels.Game;
    using System.Collections.Generic;

    public class UserService : IUserService
    {
        public bool IsAdmin(string email)
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Users
                    .FirstOrDefault(u => u.Email.ToLower() == email.ToLower())
                    .IsAdmin;
            }
            
        }

        public bool Save(string email, string password, string fullName)
        {
            using (var db = new GameStoreDbContext())
            {
                var users = db.Users;

                if(users.Any(u => u.Email.ToLower() == email.ToLower()))
                {
                    return false;
                }

                User user = new User()
                {
                    Email = email,
                    FullName = fullName,
                    Password = password,
                    IsAdmin = !users.Any()
                };

                db.Add(user);
                db.SaveChanges();

                return true;
            }
        }

        public bool Find(string email, string password)
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Users.Any(u => u.Email.ToLower() == email.ToLower() && u.Password == password);
            }
        }

        public User Find(string email)
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Users.Where(u => u.Email.ToLower() == email.ToLower()).FirstOrDefault();
            }
        }

        public int Order(HashSet<IndexViewGame> orderGames, string username)
        {
           using (var db = new GameStoreDbContext())
           {
                int userId = this.Find(username).Id;

                var user = db.Users
                    .First(u => u.Id == userId);

                var userGames = db.UserGames.Where(ug => ug.UserId == userId);

                foreach (var orderGame in orderGames)
                {
                    var game = db.Games
                        .First(g => g.Id == orderGame.Id);

                    var order = new UserGame()
                    {
                        UserId = user.Id,
                        User = user,
                        GameId = game.Id,
                        Game = game
                    };

                    if (!userGames.Any(ug => ug.UserId == userId && ug.GameId == game.Id))
                    {
                        db.Add(order);
                    }
                }

                return db.SaveChanges();
            }
        }

        public bool Exists(string email)
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Users.Any(u => u.Email.ToLower() == email.ToLower());
            }
        }
    }
}
