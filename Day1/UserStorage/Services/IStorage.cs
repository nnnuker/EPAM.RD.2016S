using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Entities;

namespace UserStorage.Services
{
    public interface IStorage
    {
        int Add(User user);
        int Search(Predicate<User> predicate);
        void Delete(User user);
        void Delete(int userId);
    }
}
