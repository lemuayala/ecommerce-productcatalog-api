using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ECommerce.ProductCatalog.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace ECommerce.ProductCatalog.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }

            services.AddDbContext<ProductCatalogDbContext>(options =>
                    options.UseSqlServer(connectionString)
                        .LogTo(Console.WriteLine, LogLevel.Information)
                        .EnableSensitiveDataLogging() // ¡CUIDADO en producción!
            );
            
            services.AddScoped<Interfaces.IUnitOfWork, Data.UnitOfWork>();
            services.AddScoped<Domain.Interfaces.Repositories.ICategoryRepository, Data.Repositories.CategoryRepository>();
            services.AddScoped<Domain.Interfaces.Repositories.IProductRepository, Data.Repositories.ProductRepository>();

            return services;
        }
    }
}