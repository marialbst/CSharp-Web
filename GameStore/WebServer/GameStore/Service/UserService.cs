namespace WebServer.GameStore.Service
{
    using System.Linq;
    using Contracts;
    using Data;
    using ViewModels.Account;
    using Data.Models;

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

        public bool Save(RegisterViewModel model)
        {
            using (var db = new GameStoreDbContext())
            {
                var users = db.Users;

                if(users.Any(u => u.Email.ToLower() == model.Email.ToLower()))
                {
                    return false;
                }

                User user = new User()
                {
                    Email = model.Email,
                    FullName = model.FullName,
                    Password = model.Password,
                    IsAdmin = !users.Any()
                };

                db.Add(user);
                db.SaveChanges();

                return true;
            }
        }

        public bool Find(LoginViewModel model)
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Users.Any(u => u.Email.ToLower() == model.Email.ToLower() && model.Password == u.Password);
            }
        }
    }
}
