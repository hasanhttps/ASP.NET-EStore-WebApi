using EStore.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using EStore.Application.Repositories;
using EStore.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EStore.Persistence;

public static class RegisterServices {

    public static void AddPersistenceRegister(this IServiceCollection services) {

        services.AddDbContext<EStoreDbContext>(options => {
            ConfigurationBuilder configurationBuilder = new();
            var builder = configurationBuilder.AddJsonFile("appsettings.json").Build();

            options.UseLazyLoadingProxies()
                   .UseSqlServer(builder.GetConnectionString("default"));
        });

        // Register all Repository in Persistence

        // All Read Repository
        services.AddScoped<IReadUserRepository, ReadUserRepository>();
        services.AddScoped<IReadRoleRepository, ReadRoleRepository>();
        services.AddScoped<IReadProductRepository, ReadProductRepository>();
        services.AddScoped<IReadInvoiceRepository, ReadInvoiceRepository>();
        services.AddScoped<IReadCategoryRepository, ReadCategoryRepository>();
        services.AddScoped<IReadUserTokenRepository, ReadUserTokenRepository>();
        services.AddScoped<IReadInvoiceItemRepository, ReadInvoiceItemRepository>();

        // All Write Repository
        services.AddScoped<IWriteUserRepository, WriteUserRepository>();
        services.AddScoped<IWriteRoleRepository, WriteRoleRepository>();
        services.AddScoped<IWriteProductRepository, WriteProductRepository>();
        services.AddScoped<IWriteInvoiceRepository, WriteInvoiceRepository>();
        services.AddScoped<IWriteCategoryRepository, WriteCategoryRepository>();
        services.AddScoped<IWriteUserTokenRepository, WriteUserTokenRepository>();
        services.AddScoped<IWriteInvoiceItemRepository, WriteInvoiceItemRepository>();
    }
}
