namespace WebServer.ByTheCake.Services
{
    using System;
    using Contracts;
    using Models;
    using Data;
    using System.Linq;
    using WebServer.ByTheCake.ViewModels.User;

    public class UserService : IUserService
    {
        public bool Create(string username, string password)
        {
            using (var db = new ByTheCakeDbContext())
            {
                if(db.Users.Any(u => u.Username == username))
                {
                    return false;
                }

                User user = new User()
                {
                    Username = username,
                    Password = password,
                    RegistrationDate = DateTime.UtcNow
                };

                db.Add(user);
                db.SaveChanges();

                return true;
            }
        }

        public bool Find(string username, string password)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db.Users.Any(u => u.Username.ToLower() == username.ToLower() && u.Password == password);
            }
        }

        public ProfileViewModel FindByUsername(string username)
        {
            using (var db = new ByTheCakeDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Username.ToLower() == username.ToLower());
                var userOrdersCount = db.Orders.Where(o => o.UserId == user.Id).Count();

                if (user == null)
                {
                    return null;
                }

                return new ProfileViewModel()
                {
                    Username = user.Username,
                    RegisteredOn = user.RegistrationDate,
                    OrdersCount = userOrdersCount
                };
            }
        }

        public User FindUserByUsername(string username)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db.Users.FirstOrDefault(u => u.Username.ToLower() == username.ToLower());
            }
        }

        public int? GetUserId(string username)
        {
            using (var db = new ByTheCakeDbContext())
            {
                 var id = db.Users
                    .Where(u => u.Username.ToLower() == username.ToLower())
                    .Select(u => u.Id)
                    .FirstOrDefault();

                return id != 0 ? (int?)id : null;
            }
        }
    }
}
