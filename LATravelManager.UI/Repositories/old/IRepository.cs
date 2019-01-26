using LATravelManager.BaseTypes;
using System.Threading.Tasks;

namespace LATravelManager.UI.Repositories
{
    public interface IRepository : IReadOnlyRepository
    {
        void Create<TEntity>(TEntity entity, string createdBy = null)
            where TEntity : BaseModel;

        void Update<TEntity>(TEntity entity, string modifiedBy = null)
            where TEntity : BaseModel;

        void Delete<TEntity>(object id)
            where TEntity : BaseModel;

        void Delete<TEntity>(TEntity entity)
            where TEntity : BaseModel;

        void Save();

        Task SaveAsync();
    }
}