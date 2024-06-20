using EStore.Domain.Entities.Concretes;
using EStore.Application.Repositories.Common;

namespace EStore.Application.Repositories;

public interface IWriteProductRepository : IWriteGenericRepository<Product> {
}
