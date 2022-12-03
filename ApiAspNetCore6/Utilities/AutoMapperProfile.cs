using ApiAspNetCore6.DTOs;
using ApiAspNetCore6.Entities;
using AutoMapper;

namespace ApiAspNetCore6.Utilities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateAuthor, Author>();
            CreateMap<Author, DisplayAuthor>();

            CreateMap<CreateBook, Book>();
            CreateMap<Book, DisplayBook>();

            CreateMap<CreateComment, Comment>();
            CreateMap<Comment, DisplayComment>();
        }
    }
}
