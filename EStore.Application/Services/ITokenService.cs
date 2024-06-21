using EStore.Domain.Helpers;
using EStore.Domain.Entities.Concretes;

namespace EStore.Application.Services;

public interface ITokenService {
    string CreateAccessToken(User user);
    RefreshToken CreateRefreshToken();
    RefreshToken CreateRepasswordToken();
    RefreshToken CreateConfirmEmailToken();
}
