﻿namespace WebServer.Server.HTTP.Contracts
{
    public interface IHttpSession
    {
        string Id { get; }

        object Get(string key);

        T Get<T>(string key);

        void Add(string key, object value);

        void Clear();

        void Clear(string key);

        bool IsAuthenticated();

        bool Contains(string key);
    }
}
