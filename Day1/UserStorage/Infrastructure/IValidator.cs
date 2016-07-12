using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Entities;

namespace UserStorage.Infrastructure
{
    public interface IValidator<T> where T : IEntity
    {
        bool IsValid(T user);
    }
}
