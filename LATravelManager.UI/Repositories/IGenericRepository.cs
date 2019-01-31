using LATravelManager.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LATravelManager.UI.Repositories
{
    public interface IGenericRepository
    {
        Task<T> GetByIdAsync<T>(int id) where T : BaseModel;

        Task<IEnumerable<T>> GetAllAsync<T>() where T : BaseModel;

        Task<IEnumerable<TEntity>> GetAllAsyncSortedByName<TEntity>() where TEntity : BaseModel, INamed;

        Task<TEntity> GetByNameAsync<TEntity>(string name) where TEntity : BaseModel, INamed;

        void Delete<TEntity>(TEntity entity) where TEntity : BaseModel;

        Task SaveAsync();

        void UpdateValues<TEntity>(TEntity entity, TEntity newEntity) where TEntity : BaseModel;

        bool HasChanges();

        void Add<T>(T model) where T : BaseModel;

        void RemoveById<TEntity>(int id) where TEntity : BaseModel;
    }
}