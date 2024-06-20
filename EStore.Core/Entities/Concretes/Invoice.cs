using EStore.Domain.Enums;
using EStore.Domain.Entities.Common;

namespace EStore.Domain.Entities.Concretes;

public class Invoice : BaseEntity {

    // Columns

    public string? InvoiceBarcode { get; set; }
    public int Barcode { get; set; }
    public InvoiceType InvoiceType { get; set; }

    // Foreign Keys

    public int CashierId { get; set; }
    public int CustomerId { get; set; }
    public int? SellInvoiceId { get; set; }

    // Navigation Properties

    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
}
