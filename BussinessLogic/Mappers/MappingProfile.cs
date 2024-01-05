using AutoMapper;
using Entity.DTOs;
using Entity.Models;
using System;
using System.Threading.Tasks;

namespace BussinessLogic.Mappers
{
    public class MappingProfile : Profile
    {
       public MappingProfile()
        {
            CreateMap<CartItem, CartItemDTO>();
            CreateMap<CartItemDTO, CartItem>();

            CreateMap<Cart, CartDTO>();
            CreateMap<CartDTO, Cart>();

            CreateMap<Author,AuthorDTO>();
            CreateMap<AuthorDTO, Author>();

            CreateMap<Publisher, PublisherDTO>();
            CreateMap<PublisherDTO, Publisher>();

            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();

            CreateMap<Order, OrderDTO>();
            CreateMap<OrderDTO, Order>();

            CreateMap<Book, BookDTO>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.FullName))
            .ForMember(dest => dest.PublisherName, opt => opt.MapFrom(src => src.Publisher.Name))
            .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(
                src => src.Categories.Select(c => c.CategoryId).ToArray()));
            CreateMap<BookDTO, Book>();
        }
    }
}
