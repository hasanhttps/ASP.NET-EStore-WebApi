using EStore.Domain.Entities.Concretes;
using EStore.Application.Repositories.Common;

namespace EStore.Application.Repositories;

public interface IReadUserRepository : IReadGenericRepository<User> {

    // Methods

    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserByUserName(string userName);
    Task<User?> GetUserByRefreshToken(string refreshToken);
    Task<User?> GetUserByRePasswordToken(string rePasswordToken);
    Task<User?> GetUserByConfirmEmailToken(string confirmEmailToken);

}
