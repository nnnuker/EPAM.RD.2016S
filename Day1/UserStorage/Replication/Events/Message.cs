using System;
using System.Collections.Generic;
using System.ComponentModel;
using UserStorage.Entities;

namespace UserStorage.Replication.Events
{
    [Serializable]
    public class Message
    {
        private IEnumerable<User> userData;
        private MessageEnum messageType;

        public IEnumerable<User> UserData
        {
            get { return userData; }
            set
            {
                if (value != null)
                {
                    userData = value;
                }
            }
        }

        public MessageEnum MessageType
        {
            get { return messageType; }
            set { messageType = value; }
        }

        public Message(IEnumerable<User> user, MessageEnum isRemoving)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (!Enum.IsDefined(typeof(MessageEnum), isRemoving))
                throw new InvalidEnumArgumentException(nameof(isRemoving), (int) isRemoving, typeof(MessageEnum));

            this.userData = user;
            this.messageType = isRemoving;
        }
    }
}