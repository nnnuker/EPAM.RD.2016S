using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Entities;

namespace UserStorage.Replication
{
    public interface ISlave<T> where T : IEntity
    {
        void OnAdd(IEnumerable<T> entities);
        void OnDelete(IEnumerable<T> entities);
    }
}
