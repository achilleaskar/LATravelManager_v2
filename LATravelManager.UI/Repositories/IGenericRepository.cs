using LATravelManager.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LATravelManager.UI.Repositories
{
    public interface IGenericRepository
    {
        Task<T> GetByIdAsync<T>(int id) where T : BaseModel;

        Task<IEnumerable<T>> GetAllAsync<T>() where T : BaseModel;

        Task SaveAsync();

        bool HasChanges();

        void Add<T>(T model) where T : BaseModel;

        void Remove<T>(T model) where T : BaseModel;
    }
}