using AutoMapper;
using Product.Application.Dtos;
using Product.Domain.Entities;

namespace Product.Application.Mappings
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<CreateProductDto, ProductItem>();
            CreateMap<RegisterDto, User>();
            CreateMap<ProductItem, ProductsResponse>();


        }
    }
}
