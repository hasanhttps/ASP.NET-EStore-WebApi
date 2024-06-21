using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using EStore.Persistence.DbContexts;
using EStore.Domain.Entities.Abstracts;
using EStore.Application.Repositories.Common;

namespace EStore.Persistence.Repositories.Common;

public class ReadGenericRepository<T> : GenericRepository<T>, IReadGenericRepository<T> where T : class, IBaseEntity, new() {

    // Constructor

    public ReadGenericRepository(EStoreDbContext context) : base(context) { }

    // Methods

    public async Task<IEnumerable<T>> GetAllAsync() {
        return _table;
    }

    public async Task<IQueryable<T>> GetByExpressionAsync(Expression<Func<T, bool>> expression) {
        return _table.Where(expression);
    }

    public async Task<T> GetByIdAsync(int id) {
        return await _table.FirstOrDefaultAsync(x => x.Id == id);
    }
}

