namespace WebServer.GameStore.Service.Contracts
{
    using System;
    using System.Collections.Generic;
    using ViewModels.Game;
    using WebServer.GameStore.Data.Models;

    public interface IGameService
    {
        IList<ListGamesViewModel> List();

        void Create(string title, string trailerId, string imageUrl, double size, decimal price, string description, DateTime releaseDate);

        AddGameViewModel Find(int id);

        IndexViewGame Get(int id);

        bool Exists(int id);

        bool Edit(int id, string title, string trailerId, string imageUrl, double size, decimal price, string description, DateTime releaseDate);

        void Delete(int id);

        IList<IndexViewGame> All();

        IList<IndexViewGame> All(int id);
    }
}
