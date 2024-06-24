namespace EStore.Domain.ViewModels;

public class GetUserVM {

    // Fields

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
}
