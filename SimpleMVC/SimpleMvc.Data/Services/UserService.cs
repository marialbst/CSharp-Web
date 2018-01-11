namespace SimpleMvc.Data.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Domain.Entities;

    public class UserService : IUserService
    {
        public bool AddUserToTheDb(string username, string password)
        {
            using (var db = new NotesDbContext())
            {
                var users = db.Users;

                if(!users.Any(u => u.Username == username))
                {
                    User user = new User
                    {
                        Username = username,
                        Password = password
                    };

                    db.Users.Add(user);
                    db.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public ICollection<string> AllUsernames()
        {
            using (var db = new NotesDbContext())
            {
                return db.Users.Select(u => u.Username).ToList();
            }
        }
    }
}
