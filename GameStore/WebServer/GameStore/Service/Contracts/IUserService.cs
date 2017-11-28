namespace WebServer.GameStore.Service.Contracts
{
    using ViewModels.Account;

    public interface IUserService
    {
        bool IsAdmin(string userName);

        bool Save(RegisterViewModel model);

        bool Find(LoginViewModel model);
    }
}
