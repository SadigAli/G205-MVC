using AutoMapper;
using Ecommerce.Data.DTOs.CategoryDTO;
using Ecommerce.Data.DTOs.ColorDTO;
using Ecommerce.Data.DTOs.ProductDTO;
using Ecommerce.Data.DTOs.SizeDTO;
using Ecommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Data.Mappers
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Color, ColorGetDTO>();
            CreateMap<ColorPostDTO, Color>().ReverseMap();

            CreateMap<Size, SizeGetDTO>();
            CreateMap<SizePostDTO, Size>().ReverseMap();

            CreateMap<Category, CategoryGetDTO>();
            CreateMap<CategoryPutDTO, Category>().ReverseMap();
            CreateMap<CategoryPostDTO, Category>().ReverseMap();

            CreateMap<Product, ProductGetDTO>()
                .ForMember(src => src.Images, dest => dest.MapFrom(x => x.ProductImages.Select(x => x.Image).ToList()))
                .ForMember(src => src.PosterImage, dest => dest.MapFrom(x => x.ProductImages.FirstOrDefault(x => x.Status == ImageStatus.Poster).Image))
                .ForMember(src => src.Category, dest => dest.MapFrom(x => x.Category.Name))
                .ForMember(x => x.Colors, dest => dest.MapFrom(x => x.ProductColors.Select(x => x.Color.Name).ToList()));

            CreateMap<Product, ProductPutDTO>()
                .ForMember(src => src.Images, dest => dest.MapFrom(x => x.ProductImages.Where(x=>x.Status == ImageStatus.Normal).Select(x => x.Image).ToList()))
                .ForMember(src => src.PosterImage, dest => dest.MapFrom(x => x.ProductImages.FirstOrDefault(x => x.Status == ImageStatus.Poster).Image))
                .ForMember(src => src.Category, dest => dest.MapFrom(x => x.Category.Name))
                .ForMember(x => x.Colors, dest => dest.MapFrom(x => x.ProductColors.Select(x => x.Color.Name).ToList()))
                .ReverseMap();

            CreateMap<ProductPostDTO, Product>();
            CreateMap<ProductColor, ProductColorGetDTO>()
                //.ForMember(src => src.Sizes, dest => dest.MapFrom(x => x.ProductColorSizes.Select(x => x.Size.Name).ToList()))
                .ForMember(src => src.Sizes, dest => dest.MapFrom(x => x.ProductColorSizes.Select(x => new ProductColorSizeDTO
                {
                    Id = x.Id,
                    Name = x.Size.Name
                }).ToList()))
                .ForMember(x=>x.TotalCount, dest=>dest.MapFrom(x=>x.ProductColorSizes.Sum(x=>x.Count)));
                   

            CreateMap<ProductColorPostDTO, ProductColor>().ReverseMap();
            CreateMap<ProductColorSizeDTO, ProductColorSize>().ReverseMap()
                .ForMember(src=>src.Name,dest=>dest.MapFrom(x=>x.Size.Name));

        }
    }
}
