using EStore.Persistence.DbContexts;
using EStore.Application.Repositories;
using EStore.Domain.Entities.Concretes;
using EStore.Persistence.Repositories.Common;

namespace EStore.Persistence.Repositories;

public class WriteCategoryRepository : WriteGenericRepository<Category>, IWriteCategoryRepository {

    // Constructor

    public WriteCategoryRepository(EStoreDbContext context) : base(context) { }

}
