using Microsoft.EntityFrameworkCore;
using EStore.Domain.Entities.Concretes;

namespace EStore.Persistence.DbContexts;

public class EStoreDbContext : DbContext {

    // Constructor

    public EStoreDbContext(DbContextOptions<EStoreDbContext> options) : base(options) { }

    // Tables

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Invoice> Invoices { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<UserToken> UserTokens { get; set; }
    public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }
}
