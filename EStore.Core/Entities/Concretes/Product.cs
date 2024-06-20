using EStore.Domain.Entities.Common;

namespace EStore.Domain.Entities.Concretes;

public class Product : BaseEntity {

    // Columns

    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public string? ImageUrl { get; set; }

    // Foreign Key

    public int CategoryId { get; set; }

    // Navigation Property

    public virtual Category Category { get; set; }
    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
}
