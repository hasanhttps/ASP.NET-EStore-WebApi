using EStore.Persistence.DbContexts;
using EStore.Application.Repositories;
using EStore.Domain.Entities.Concretes;
using EStore.Persistence.Repositories.Common;

namespace EStore.Persistence.Repositories;

public class WriteProductRepository : WriteGenericRepository<Product>, IWriteProductRepository {

    // Constructor

    public WriteProductRepository(EStoreDbContext context) : base(context) { }
}
