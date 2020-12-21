using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IRepository<T> where T : class
    {
        Task<IList<T>> GetAll();
        Task<T> Get(int id);
        Task Add(T entity);
        void Remove(T entity);
        void Update(T entity);
        Task<bool> Save();
    }
}
