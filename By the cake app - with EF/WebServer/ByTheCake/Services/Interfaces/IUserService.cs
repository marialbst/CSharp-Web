namespace WebServer.ByTheCake.Services.Interfaces
{
    using ViewModels.User;

    public interface IUserService
    {
        bool Create(string username, string password);

        bool Find(string username, string password);

        ProfileViewModel FindByUsername(string username);
    }
}
