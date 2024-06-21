using EStore.Persistence.DbContexts;
using EStore.Application.Repositories;
using EStore.Domain.Entities.Concretes;
using EStore.Persistence.Repositories.Common;

namespace EStore.Persistence.Repositories;

public class WriteRoleRepository : WriteGenericRepository<Role>, IWriteRoleRepository {

    // Constructor

    public WriteRoleRepository(EStoreDbContext context) : base(context) { }
}
