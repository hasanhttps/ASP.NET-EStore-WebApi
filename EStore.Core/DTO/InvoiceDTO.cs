using EStore.Domain.Enums;

namespace EStore.Domain.DTO;

public class InvoiceDTO {

    // Fields

    public string? InvoiceBarcode { get; set; }
    public int Barcode { get; set; }
    public InvoiceType InvoiceType { get; set; }

    // Foreign Keys

    public int CashierId { get; set; }
    public int CustomerId { get; set; }
}
