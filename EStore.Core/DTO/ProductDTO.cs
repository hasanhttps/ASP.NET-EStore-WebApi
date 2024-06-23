using EStore.Domain.Entities.Concretes;

namespace EStore.Domain.DTO;

public class ProductDTO {

    // Fields

    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public string? ImageUrl { get; set; }
    public string Category { get; set; }
}
