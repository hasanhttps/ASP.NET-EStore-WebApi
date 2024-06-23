using EStore.Domain.Helpers;
using EStore.Domain.Entities.Concretes;

namespace EStore.Application.Services;

public interface ITokenService {
    TokenCredentials CreateAccessToken(User user);
    TokenCredentials CreateRefreshToken();
    TokenCredentials CreateRepasswordToken();
    TokenCredentials CreateConfirmEmailToken();
}
