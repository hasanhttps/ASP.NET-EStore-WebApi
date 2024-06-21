using EStore.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using EStore.Application.Repositories;
using EStore.Domain.Entities.Concretes;
using EStore.Persistence.Repositories.Common;

namespace EStore.Persistence.Repositories;

public class ReadProductRepository : ReadGenericRepository<Product>, IReadProductRepository {

    // Constructor

    public ReadProductRepository(EStoreDbContext context) : base(context) { }

    // Methods

    public async Task<Category>? GetCategoryById(int id) {
        var product = await _table.Where(p => p.Id == id).FirstOrDefaultAsync();
        return product.Category;
    }
}
