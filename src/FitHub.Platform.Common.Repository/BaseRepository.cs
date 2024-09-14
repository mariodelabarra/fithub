using FitHub.Platform.Common.Domain;
using MongoDB.Entities;

namespace FitHub.Platform.Common.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetByIdAsync(string id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(string id);
    }

    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        public BaseRepository() { }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DB.Find<TEntity>().ExecuteAsync();
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            return await DB.Find<TEntity>().OneAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await entity.SaveAsync();
        }

        public async Task DeleteAsync(string id)
        {
            await DB.DeleteAsync<TEntity>(id);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await entity.SaveAsync();
        }
    }
}
