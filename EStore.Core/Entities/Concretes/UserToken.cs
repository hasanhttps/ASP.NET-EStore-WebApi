using EStore.Domain.Entities.Common;

namespace EStore.Domain.Entities.Concretes;

public class UserToken : BaseEntity {

    // Columns

    // Acces Token

    public string? AccesToken { get; set; }
    public DateTime? AccesTokenExpireTime { get; set; }
    public DateTime? AccesTokenCreateTime { get; set; } = DateTime.Now;

    // Refresh Token

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpireTime { get; set; }
    public DateTime? RefreshTokenCreateTime { get; set; } = DateTime.Now;

    // RePassword Token

    public string? RePasswordToken { get; set; }
    public DateTime? RePasswordTokenExpireTime { get; set; }
    public DateTime? RePasswordTokenCreateTime { get; set; } = DateTime.Now;

    // Confirm Email Token

    public string? ConfirmEmailToken { get; set; }
    public DateTime? ConfirmEmailTokenExpireTime { get; set; }
    public DateTime? ConfirmEmailTokenCreateTime { get; set; } = DateTime.Now;

    // Navigation Property

    public virtual User? User { get; set; }
}
