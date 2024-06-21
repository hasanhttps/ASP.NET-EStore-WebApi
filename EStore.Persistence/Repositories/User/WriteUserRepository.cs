using EStore.Persistence.DbContexts;
using EStore.Application.Repositories;
using EStore.Domain.Entities.Concretes;
using EStore.Persistence.Repositories.Common;

namespace EStore.Persistence.Repositories;

public class WriteUserRepository : WriteGenericRepository<User>, IWriteUserRepository {

    // Constructor

    public WriteUserRepository(EStoreDbContext context) : base(context) { }
}