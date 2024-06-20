using EStore.Domain.Entities.Common;

namespace EStore.Domain.Entities.Concretes;

public class InvoiceItem : BaseEntity {

    // Columns

    public decimal Quantity { get; set; }

    // Foreign Key

    public int ProductId { get; set; }
    public int InvoiceId { get; set; }

    // Navigation Properties

    public virtual Product Product { get; set; }
    public virtual Invoice Invoice { get; set; }
}
