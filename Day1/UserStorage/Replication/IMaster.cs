using System;
using UserStorage.Entities;
using UserStorage.Replication.Events;
using UserStorage.Services;

namespace UserStorage.Replication
{
    public interface IMaster : IUserStorage
    {
        event EventHandler<MessageEventArgs> AddEvent;
        event EventHandler<MessageEventArgs> DeleteEvent;
    }
}