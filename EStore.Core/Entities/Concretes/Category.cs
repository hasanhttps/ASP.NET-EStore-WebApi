using EStore.Domain.Entities.Common;

namespace EStore.Domain.Entities.Concretes;

public class Category : BaseEntity {

    // Columns

    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    // Navigation Property

    public virtual ICollection<Product> Products { get; set; }
}
