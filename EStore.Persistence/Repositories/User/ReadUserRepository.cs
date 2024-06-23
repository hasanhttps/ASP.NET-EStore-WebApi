using EStore.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using EStore.Application.Repositories;
using EStore.Domain.Entities.Concretes;
using EStore.Persistence.Repositories.Common;

namespace EStore.Persistence.Repositories;

public class ReadUserRepository : ReadGenericRepository<User>, IReadUserRepository {

    // Constructor

    public ReadUserRepository(EStoreDbContext context) : base(context) { }

    // Methods

    public async Task<User?> GetUserByEmail(string email) {
        return await _table.FirstOrDefaultAsync(p => p.Email == email);
    }

    public async Task<User?> GetUserByRefreshToken(string refreshToken) {
        return await _table.SingleOrDefaultAsync(p => p.UserToken.RefreshToken == refreshToken);
    }

    public async Task<User?> GetUserByRePasswordToken(string rePasswordToken) {
        return await _table.FirstOrDefaultAsync(p => p.UserToken.RePasswordToken == rePasswordToken);
    }

    public async Task<User?> GetUserByConfirmEmailToken(string confirmEmailToken) {
        return await _table.FirstOrDefaultAsync(p => p.UserToken.ConfirmEmailToken == confirmEmailToken);
    }

    public async Task<User?> GetUserByAccessToken(string accessToken) {
        return await _table.FirstOrDefaultAsync(p => p.UserToken.AccesToken == accessToken);
    }

    public async Task<User?> GetUserByUserName(string userName) {
        return await _table.FirstOrDefaultAsync(p => p.UserName == userName);
    }
}