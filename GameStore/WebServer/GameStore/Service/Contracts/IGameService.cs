namespace WebServer.GameStore.Service.Contracts
{
    using System.Collections.Generic;
    using ViewModels.Game;

    public interface IGameService
    {
        IList<ListGamesViewModel> List();
    }
}
