namespace GameStore.Services
{
    using Contracts;
    using Data.Entities;
    using Data;
    using System.Linq;

    public class UserService : IUserService
    {
        public bool Create(string email, string password, string name)
        {
            using (var db = new GameStoreMvcDbContext())
            {
                if(db.Users.Any(u => u.Email.ToLower() == email.ToLower()))
                {
                    return false;
                }

                User user = new User
                {
                    Email = email,
                    Password = password,
                    Name = name,
                    IdAdmin = !db.Users.Any()
                };

                db.Users.Add(user);
                db.SaveChanges();

                return true;
            }
        }

        public bool Exists(string name)
        {
            using (var db = new GameStoreMvcDbContext())
            {
                return db.Users
                    .Any(u => u.Email.ToLower() == name.ToLower());
            }
        }

        public bool Find(string email, string password)
        {
            using (var db = new GameStoreMvcDbContext())
            {
                return db.Users
                    .Any(u => u.Email.ToLower() == email.ToLower() 
                           && u.Password == password);
            }
        }

        public bool IsAdmin(string name)
        {
            using (var db = new GameStoreMvcDbContext())
            {
                var user = db.Users
                    .FirstOrDefault(u => u.Email.ToLower() == name.ToLower());

                if(user == null)
                {
                    return false;
                }

                return user.IdAdmin;
            }
        }
    }
}
