using EStore.Persistence.DbContexts;
using EStore.Application.Repositories;
using EStore.Domain.Entities.Concretes;
using EStore.Persistence.Repositories.Common;

namespace EStore.Persistence.Repositories;

public class ReadInvoiceRepository : ReadGenericRepository<Invoice>, IReadInvoiceRepository {

    // Constructor

    public ReadInvoiceRepository(EStoreDbContext context) : base(context) { }
}
