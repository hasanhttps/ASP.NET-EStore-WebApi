using EStore.Persistence.DbContexts;
using EStore.Application.Repositories;
using EStore.Domain.Entities.Concretes;
using EStore.Persistence.Repositories.Common;

namespace EStore.Persistence.Repositories;

public class ReadUserTokenRepository : ReadGenericRepository<UserToken>, IReadUserTokenRepository {

    // Constructor

    public ReadUserTokenRepository(EStoreDbContext context) : base(context) { }
}
