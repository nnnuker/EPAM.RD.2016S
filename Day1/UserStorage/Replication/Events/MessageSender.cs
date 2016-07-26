using System;
using System.Collections.Generic;

namespace UserStorage.Replication.Events
{
    public class MessageSender : IMessageSender
    {
        public void SendMessage(IEnumerable<ISlave> slaves, Message message)
        {
            if (slaves == null) throw new ArgumentNullException(nameof(slaves));
            if (message == null) throw new ArgumentNullException(nameof(message));

            foreach (var slave in slaves)
            {
                slave.OnNotifyUser(message);
            }
        }
    }
}