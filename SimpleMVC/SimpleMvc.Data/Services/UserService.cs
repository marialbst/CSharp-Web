namespace SimpleMvc.Data.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;

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

        public IDictionary<int, string> AllUsers()
        {
            using (var db = new NotesDbContext())
            {
                return db.Users.Include(u => u.Notes).ToDictionary(u => u.Id,u => u.Username);
            }
        }

        public User GetUserById(int id)
        {
            using (var db = new NotesDbContext())
            {
                return db.Users.Include(u => u.Notes).FirstOrDefault(u => u.Id == id);
            }
        }

        public void AddNoteToUser(int userId, string title, string content)
        {
            using (var db = new NotesDbContext())
            {
                User user = db.Users.Find(userId);

                Note note = new Note
                {
                    Title = title,
                    Content = content
                };

                user.Notes.Add(note);
                db.SaveChanges();
            }
        }
    }
}
