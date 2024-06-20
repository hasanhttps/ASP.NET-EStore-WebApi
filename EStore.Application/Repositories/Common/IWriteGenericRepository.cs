using EStore.Domain.Entities.Abstracts;

namespace EStore.Application.Repositories.Common;

public interface IWriteGenericRepository<T> : IGenericRepository<T> where T : class, IBaseEntity, new() {

    // Methods

    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task DeleteAsync(T entity);
    Task SaveChangeAsync();
}
