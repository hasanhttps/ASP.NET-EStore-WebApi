using EStore.Persistence.DbContexts;
using EStore.Application.Repositories;
using EStore.Domain.Entities.Concretes;
using EStore.Persistence.Repositories.Common;

namespace EStore.Persistence.Repositories;

public class WriteInvoiceRepository : WriteGenericRepository<Invoice>, IWriteInvoiceRepository {

    // Constructor

    public WriteInvoiceRepository(EStoreDbContext context) : base(context) { }
}
