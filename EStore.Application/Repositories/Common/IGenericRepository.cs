using EStore.Domain.Entities.Abstracts;

namespace EStore.Application.Repositories.Common;

public interface IGenericRepository<T> where T : class, IBaseEntity, new() {

}
