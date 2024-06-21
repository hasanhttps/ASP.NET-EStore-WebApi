using EStore.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using EStore.Domain.Entities.Abstracts;
using EStore.Application.Repositories.Common;

namespace EStore.Persistence.Repositories.Common;

public class WriteGenericRepository<T> : GenericRepository<T>, IWriteGenericRepository<T> where T : class, IBaseEntity, new() {

    // Constructor

    public WriteGenericRepository(EStoreDbContext context) : base(context) { }

    // Methods

    public async Task AddAsync(T entity) {
        await _table.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<T> entities) {
        await _table.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {

        var entity = await _table.FirstOrDefaultAsync(x => x.Id == id);
        if (entity != null)
            _table.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity) {
        _table.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangeAsync() {
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity) {
        _table.Update(entity);
        await _context.SaveChangesAsync();
    }
}
