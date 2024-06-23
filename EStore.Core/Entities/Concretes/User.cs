using EStore.Domain.Entities.Common;

namespace EStore.Domain.Entities.Concretes;

public class User : BaseEntity {

    // Columns

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? UserName { get; set; }
    public bool? ConfirmEmail { get; set; }
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }

    // Foreign Key

    public int RoleId { get; set; }
    public int UserTokenId { get; set; }

    // Navigation Properties

    public virtual Role Role { get; set; }
    public virtual UserToken UserToken { get; set; }
}
