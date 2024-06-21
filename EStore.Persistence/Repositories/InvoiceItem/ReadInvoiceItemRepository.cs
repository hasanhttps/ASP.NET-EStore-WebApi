using EStore.Persistence.DbContexts;
using EStore.Application.Repositories;
using EStore.Domain.Entities.Concretes;
using EStore.Persistence.Repositories.Common;

namespace EStore.Persistence.Repositories;

public class ReadInvoiceItemRepository : ReadGenericRepository<InvoiceItem>, IReadInvoiceItemRepository {

    // Constructor

    public ReadInvoiceItemRepository(EStoreDbContext context) : base(context) { }
}
