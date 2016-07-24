using System.Collections.Generic;
using UserStorage.Entities;

namespace UserStorage.Services
{
    public interface IUserStorage : IStorage<User>
    {
        int[] SearchByName(string firstName, string lastName);

        int[] SearchByPersonalId(string personalId);

        int[] SearchByVisaCountry(string country);
    }
}