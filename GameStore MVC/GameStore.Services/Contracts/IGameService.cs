namespace GameStore.Services.Contracts
{
    using System.Collections.Generic;
    using GameStore.Services.Models;

    public interface IGameService
    {
        ICollection<AllGamesModel> All();
    }
}
