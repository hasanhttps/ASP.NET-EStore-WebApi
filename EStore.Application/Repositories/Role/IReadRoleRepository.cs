using EStore.Domain.Entities.Concretes;
using EStore.Application.Repositories.Common;

namespace EStore.Application.Repositories;

public interface IReadRoleRepository : IReadGenericRepository<Role> {

    // Methods

    Task<Role?> GetByRoleName(string roleName);
    Task<IEnumerable<User>?> GetUsersByRoleName(string roleName);
}
