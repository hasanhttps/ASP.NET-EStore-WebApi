using System.ComponentModel.DataAnnotations;

namespace EStore.Domain.DTOs;

public class ResetPasswordDTO {

    // Fields

    public string Password { get; set; }
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}
