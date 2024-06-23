namespace EStore.Domain.DTO;

public class InvoiceItemDTO {

    // Fields

    public decimal Quantity { get; set; }

    // Foreign Key

    public int ProductId { get; set; }
    public int InvoiceId { get; set; }
}
