namespace WebServer.GameStore.Service.Contracts
{
    using ViewModels.Account;
    using Data.Models;

    public interface IUserService
    {
        bool IsAdmin(string userName);

        bool Save(string email, string password, string fullName);

        bool Find(string email, string password);

        User Find(string email);
    }
}
