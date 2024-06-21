using EStore.Persistence.DbContexts;
using EStore.Application.Repositories;
using EStore.Domain.Entities.Concretes;
using EStore.Persistence.Repositories.Common;

namespace EStore.Persistence.Repositories;

public class WriteUserTokenRepository : WriteGenericRepository<UserToken>, IWriteUserTokenRepository {

    // Constructor

    public WriteUserTokenRepository(EStoreDbContext context) : base(context) { }
}
