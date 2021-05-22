using AutoMapper;
using Backend.DTO;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Weather, WeatherToUpdateDto>()
                .ReverseMap();

            CreateMap<Order, OrderDTO>()
                .ReverseMap();

            CreateMap<OrderedProduct, OrderedProductDTO>()
                .ReverseMap();

            CreateMap<Product, ProductDTO>()
                .ReverseMap();
            
            CreateMap<ProductPrice, ProductPriceDTO>()
               .ReverseMap();

            CreateMap<Coupon, CouponDTO>()
               .ReverseMap();

            CreateMap<User, UserDTO>()
               .ReverseMap();

            CreateMap<ProductCategory, ProductCategoryDTO>()
                .ReverseMap();
            CreateMap<ProductImage, ProductImageDTO>()
               .ReverseMap();
        }
    }
}