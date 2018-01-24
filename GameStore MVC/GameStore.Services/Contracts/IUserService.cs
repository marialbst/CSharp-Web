﻿namespace GameStore.Services.Contracts
{
    public interface IUserService
    {
        bool Create(string email, string password, string name);
        bool Find(string email, string password);
    }
}
