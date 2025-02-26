using AutoMapper;
using Ecom.Core.Entities.Product;
using Ecom.Core.Models.Product;

namespace Ecom.Core.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductDto>()
            .ForMember(x => x.CategoryName, op => op.MapFrom(src => src.Category.Name))
            .ReverseMap();
            CreateMap<Photo, PhotoDto>().ReverseMap();
            CreateMap<AddProductDto, Product>()
                .ForMember(a=>a.Photos,op=>op.Ignore())
                .ReverseMap();

            CreateMap<UpdateProductDto, Product>()
               .ForMember(a => a.Photos, op => op.Ignore())
               .ReverseMap();

        }
    }
}
