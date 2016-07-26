using System.Collections.Generic;

namespace UserStorage.Replication.Events
{
    public interface IMessageSender
    {
        void SendMessage(IEnumerable<ISlave> slaves, Message message);
    }
}