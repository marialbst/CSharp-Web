namespace WebServer.GameStore.Service.Contracts
{
    using ViewModels.Account;
    using Data.Models;
    using WebServer.GameStore.ViewModels.Game;
    using System.Collections.Generic;

    public interface IUserService
    {
        bool IsAdmin(string userName);

        bool Save(string email, string password, string fullName);

        bool Find(string email, string password);

        User Find(string email);

        bool Exists(string email);

        int Order(HashSet<IndexViewGame> orderGames, string username);
    }
}
