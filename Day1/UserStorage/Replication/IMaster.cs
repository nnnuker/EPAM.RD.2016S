using System;
using UserStorage.Entities;
using UserStorage.Replication.Events;
using UserStorage.Services;

namespace UserStorage.Replication
{
    public interface IMaster : IUserStorage
    {
        void SubscribeToAddUser(ISlave slave);
        void SubscribeToDeleteUser(ISlave slave);
    }
}