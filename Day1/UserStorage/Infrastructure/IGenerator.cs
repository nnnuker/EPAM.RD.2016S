using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Infrastructure
{
    public interface IGenerator
    {
        int Get();
        void Initialize(int? last);
    }
}
