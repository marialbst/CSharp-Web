namespace WebServer.ByTheCake.Services
{
    using System;
    using Interfaces;
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

                db.Users.Add(user);
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

                if (user == null)
                {
                    return null;
                }

                return new ProfileViewModel()
                {
                    Username = user.Username,
                    RegisteredOn = user.RegistrationDate,
                    OrdersCount = user.Orders.Count()
                };
            }
        }
    }
}
