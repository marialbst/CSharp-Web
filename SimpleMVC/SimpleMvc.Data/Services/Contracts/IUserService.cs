namespace SimpleMvc.Data.Services.Contracts
{
    using System.Collections.Generic;
    using Domain.Entities;

    public interface IUserService
    {
        bool AddUserToTheDb(string username, string password);
        IDictionary<int, string> AllUsers();
        User GetUserById(int id);
        void AddNoteToUser(int userId, string title, string content);
    }
}
