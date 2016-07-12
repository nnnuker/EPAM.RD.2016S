using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Entities;

namespace UserStorage.Infrastructure
{
    public class ValidatorUsers : IValidator<User>
    {
        public bool IsValid(User user)
        {
            throw new NotImplementedException();
        }
    }
}
