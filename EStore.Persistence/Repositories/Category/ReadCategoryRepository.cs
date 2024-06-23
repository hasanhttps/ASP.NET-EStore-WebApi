using Microsoft.EntityFrameworkCore;
using EStore.Persistence.DbContexts;
using EStore.Application.Repositories;
using EStore.Domain.Entities.Concretes;
using EStore.Persistence.Repositories.Common;

namespace EStore.Persistence.Repositories;

public class ReadCategoryRepository : ReadGenericRepository<Category>, IReadCategoryRepository {

    // Constructor

    public ReadCategoryRepository(EStoreDbContext context) : base(context) { }

    // Methods

    public async Task<Category?> GetCategoryByName(string name) {
        return await _table.FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task<IEnumerable<Product>?> GetAllProductsByName(string name) {
        var category = await _table.FirstOrDefaultAsync(p => p.Name == name);
        return category.Products;
    }

    public async Task<IEnumerable<Product>> GetAllProductsById(int categoryId) {
        var products = _table.Include(x => x.Products).FirstOrDefault(x => x.Id == categoryId)?.Products;
        return products;
    }
}