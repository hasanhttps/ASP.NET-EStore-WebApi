using System.ComponentModel.DataAnnotations;

namespace EStore.Domain.ViewModels;

public class UpdateProductVM {

    // Fields

    [Required]
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }

    // Foreign Key

    public int CategoryId { get; set; }
}
