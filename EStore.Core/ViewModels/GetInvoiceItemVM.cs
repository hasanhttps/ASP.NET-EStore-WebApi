using EStore.Domain.Entities.Concretes;

namespace EStore.Domain.ViewModels;

public class GetInvoiceItemVM {

    // Fields

    public decimal Quantity { get; set; }
    public virtual GetProductVM Product { get; set; }
    public virtual GetInvoiceVM Invoice { get; set; }
}
