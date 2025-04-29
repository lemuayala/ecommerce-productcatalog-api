using ECommerce.ProductCatalog.Application;
using ECommerce.ProductCatalog.Application.Interfaces;
using ECommerce.ProductCatalog.Application.Interfaces.Repositories;
using ECommerce.ProductCatalog.Application.Interfaces.Services;
using ECommerce.ProductCatalog.Infrastructure.Data;    
using ECommerce.ProductCatalog.Infrastructure.Data.Repositories; 
using Microsoft.EntityFrameworkCore;              

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

builder.Services.AddDbContext<ProductCatalogDbContext>(options =>
        options.UseSqlServer(connectionString) 
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging() // Muestra valores de parámetros (¡CUIDADO en producción!)
);

// Inyeccion de dependencias 
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
  
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ProductCatalogDbContext>();
        dbContext.Database.Migrate();
     }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
