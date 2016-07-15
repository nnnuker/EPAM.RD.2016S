using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Entities;

namespace UserStorage.Storages
{
    public interface IRepository<T> where T : IEntity
    {
        void Add(T entity);

        T[] Get(Predicate<T> predicate);

        void Delete(int entityId);
    }
}
