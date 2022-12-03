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

            CreateMap<CreateBook, Book>()
                .ForMember(book => book.AuthorsBooks, options => options.MapFrom(MapAuthorsBooks));
            CreateMap<Book, DisplayBook>()
                .ForMember(displayBook => displayBook.Authors, options => options.MapFrom(MapDisplayAuthorsBooks));

            CreateMap<CreateComment, Comment>();
            CreateMap<Comment, DisplayComment>();
        }

        private List<AuthorBook> MapAuthorsBooks(CreateBook createBook, Book book)
        {
            var result = new List<AuthorBook>();
            if(createBook.AuthorsIds == null)
            {
                return result;
            }
            foreach (var authorId in createBook.AuthorsIds)
            {
                result.Add(new AuthorBook() { AuthorId = authorId });
            }
            return result;
        }

        private List<DisplayAuthor> MapDisplayAuthorsBooks(Book book, DisplayBook displayBook)
        {
            var result = new List<DisplayAuthor>();
            if(book.AuthorsBooks == null)
            {
                return result;
            }
            foreach (var authorBook in book.AuthorsBooks)
            {
                result.Add(new DisplayAuthor { 
                    Id = authorBook.AuthorId, 
                    Name = authorBook.Author.Name 
                });
            }
            return result;
        }
    }
}
