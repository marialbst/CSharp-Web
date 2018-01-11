using System.Collections.Generic;

namespace SimpleMvc.Data.Services.Contracts
{
    public interface IUserService
    {
        bool AddUserToTheDb(string username, string password);
        ICollection<string> AllUsernames();
    }
}
