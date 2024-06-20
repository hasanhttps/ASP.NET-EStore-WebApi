using EStore.Domain.Entities.Concretes;
using EStore.Application.Repositories.Common;

namespace EStore.Application.Repositories;

public interface IReadCategoryRepository : IReadGenericRepository<Category> {

    // Methods

    Task<IEnumerable<Product>> GetAllProductsById(int id);
}
