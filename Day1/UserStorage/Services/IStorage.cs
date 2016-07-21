using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Entities;

namespace UserStorage.Services
{
    public interface IStorage<T> where T : IEntity
    {
        int Add(T entity);

        IEnumerable<int> SearchByName(string firstName, string lastName);

        IEnumerable<int> SearchByPersonalId(string personalId);

        IEnumerable<int> SearchByVisaCountry(string country);

        void Delete(T entity);

        void Delete(int entityId);

        void Save();
    }
}
