using System.Collections.Generic;
using UserStorage.Entities;
using UserStorage.Replication.Events;
using UserStorage.Services;

namespace UserStorage.Replication
{
    public interface ISlave : IUserStorage
    {
        void OnNotifyUser(Message message);
    }
}