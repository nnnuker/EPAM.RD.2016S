using System;
using UserStorage.Entities;

namespace UserStorage.Replication.Events
{
    [Serializable]
    public class MessageEventArgs : EventArgs
    {
        private User userData;

        public User UserData
        {
            get { return userData; }
            set { userData = value; }
        }

        public MessageEventArgs(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            this.userData = user;
        }
    }
}