using EStore.Persistence.DbContexts;
using EStore.Application.Repositories;
using EStore.Domain.Entities.Concretes;
using EStore.Persistence.Repositories.Common;

namespace EStore.Persistence.Repositories;

public class ReadRoleRepository : ReadGenericRepository<Role>, IReadRoleRepository {

    // Constructor

    public ReadRoleRepository(EStoreDbContext context) : base(context) { }
}
