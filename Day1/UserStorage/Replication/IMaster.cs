using UserStorage.Entities;

namespace UserStorage.Replication
{
    public interface IMaster<T> where T : IEntity
    {
        void Subscribe(ISlave<T> slave);
    }
}