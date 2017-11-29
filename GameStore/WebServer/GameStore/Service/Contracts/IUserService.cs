namespace WebServer.GameStore.Service.Contracts
{
    using ViewModels.Account;

    public interface IUserService
    {
        bool IsAdmin(string userName);

        bool Save(string email, string password, string fullName);

        bool Find(string email, string password);
    }
}
