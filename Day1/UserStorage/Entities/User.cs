using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Infrastructure;

namespace UserStorage.Entities
{
    public class User : IEntity, IEquatable<User>
    {
        #region Properties
        public int Id { get; set; }

        public string PersonalId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public GenderEnum Gender { get; set; }

        public VisaRecord Visa { get; set; }
        #endregion

        #region Public methods
        public bool Equals(User other)
        {
            if (other == null)
            {
                return false;
            }

            return this.FirstName == other.FirstName && this.LastName == other.LastName;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as User);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;

                hash = (hash * 16777619) ^ Id.GetHashCode();
                hash = (hash * 16777619) ^ CheckOnNull(PersonalId);
                hash = (hash * 16777619) ^ CheckOnNull(FirstName);
                hash = (hash * 16777619) ^ CheckOnNull(LastName);
                hash = (hash * 16777619) ^ DateOfBirth.GetHashCode();
                hash = (hash * 16777619) ^ Gender.GetHashCode();
                hash = (hash * 16777619) ^ Visa.GetHashCode();

                return hash;
            }
        }
        #endregion

        #region Private methods
        private int CheckOnNull(object obj)
        {
            if (obj == null)
                return 0;

            return obj.GetHashCode();
        }
        #endregion
    }
}
