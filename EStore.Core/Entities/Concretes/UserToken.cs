using EStore.Domain.Entities.Common;

namespace EStore.Domain.Entities.Concretes;

public class UserToken : BaseEntity {

    // Columns

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

    // Foreign Key

    public int UserId { get; set; }
}
