using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidMvcSharePoint.Interfaces
{
    public interface IRepositoryContext : IDisposable
    {
        T Find<T>(int id) where T : class, new();

        IEnumerable<T> Get<T>() where T : class, new();
    }
}
