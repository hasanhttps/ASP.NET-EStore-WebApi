using EStore.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using EStore.Domain.Entities.Abstracts;
using EStore.Application.Repositories.Common;

namespace EStore.Persistence.Repositories.Common;

public class GenericRepository<T> : IGenericRepository<T> where T : class, IBaseEntity, new() {

    // Fields

    protected DbSet<T> _table;
    protected readonly EStoreDbContext _context;

    // Constrctor

    public GenericRepository(EStoreDbContext context) { 
        _context = context;
        _table = _context.Set<T>();
    }
}
