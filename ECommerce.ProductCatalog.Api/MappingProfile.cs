using AutoMapper;
using ECommerce.ProductCatalog.Api.Dtos.Category;
using ECommerce.ProductCatalog.Api.Dtos.Product;
using ECommerce.ProductCatalog.Domain.Entities;

namespace ECommerce.ProductCatalog.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
        }
    }
}