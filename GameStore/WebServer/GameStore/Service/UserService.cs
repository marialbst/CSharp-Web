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
    }
}
