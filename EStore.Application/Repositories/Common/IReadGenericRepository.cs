using EStore.Domain.Entities.Abstracts;

namespace EStore.Application.Repositories.Common;

public interface IReadGenericRepository<T> : IGenericRepository<T> where T : class, IBaseEntity, new()   {

    // Methods

    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
}
