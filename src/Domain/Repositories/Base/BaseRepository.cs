namespace Domain.Repositories.Base;

public interface IRepository<TEntity> where TEntity : class
{
    Task AddAsync(TEntity entity);
    Task<List<TEntity>> Get();
    Task<TEntity> GetEntityByIdAsync(string id);
    Task<bool> UpdateAsync(string id, TEntity entity);
    Task<bool> DeleteAsync(string id);
}