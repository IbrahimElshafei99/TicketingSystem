using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Core.Interfaces
{
    public interface IGenericsRepo<T> where T : class
    {
        Task<T> GetbyId(int id);
        Task AddRecord(T entity);
        Task UpdateRecord(T entity);
    }
}
