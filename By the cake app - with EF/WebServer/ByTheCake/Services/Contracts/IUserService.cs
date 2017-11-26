namespace WebServer.ByTheCake.Services.Contracts
{
    using ViewModels.User;
    using Models;

    public interface IUserService
    {
        bool Create(string username, string password);

        bool Find(string username, string password);

        ProfileViewModel FindByUsername(string username);

        int? GetUserId(string username);

        User FindUserByUsername(string username);
    }
}
