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
            if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName)
                || string.IsNullOrEmpty(user.PersonalId) || string.IsNullOrEmpty(user.Visa.Country)
                || user.DateOfBirth > DateTime.Now || !Enum.IsDefined(typeof(GenderEnum), user.Gender))
            {
                return false;
            }

            return true;
        }
    }
}
