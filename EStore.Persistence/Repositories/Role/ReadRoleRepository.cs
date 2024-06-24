using EStore.Persistence.DbContexts;
using EStore.Application.Repositories;
using EStore.Domain.Entities.Concretes;
using EStore.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace EStore.Persistence.Repositories;

public class ReadRoleRepository : ReadGenericRepository<Role>, IReadRoleRepository {

    // Constructor

    public ReadRoleRepository(EStoreDbContext context) : base(context) { }

    // Methods

    public async Task<Role?> GetByRoleName(string roleName) {
        return await _table.Where(p => p.RoleName == roleName).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<User>?> GetUsersByRoleName(string roleName) {
        var role = await GetByRoleName(roleName);
        return role.Users;
    }
}
