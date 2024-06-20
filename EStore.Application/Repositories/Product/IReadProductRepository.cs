using EStore.Domain.Entities.Concretes;
using EStore.Application.Repositories.Common;

namespace EStore.Application.Repositories;

public interface IReadProductRepository : IReadGenericRepository<Product> {

    // Methods

    Task<Category>? GetCategoryById (int id);
}
