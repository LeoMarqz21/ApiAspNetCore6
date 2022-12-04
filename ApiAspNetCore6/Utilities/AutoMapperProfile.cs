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
            CreateMap<UpdateAuthor, Author>();
            CreateMap<Author, DisplayAuthorWithBooks>()
                .ForMember(
                displayAuthor => displayAuthor.Books, 
                options => options.MapFrom(MapBooksToDisplayBooks)
                );

            CreateMap<CreateBook, Book>()
                .ForMember(
                book => book.AuthorsBooks, 
                options => options.MapFrom(MapAuthorsBooks)
                );
            CreateMap<Book, DisplayBook>();
            CreateMap<Book, DisplayBookWithAuthors>()
                .ForMember(
                displayBook => displayBook.Authors, 
                options => options.MapFrom(MapBookAuthorToDisplayAuthors)
                );

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

        private List<DisplayAuthor> MapBookAuthorToDisplayAuthors(Book book, DisplayBook displayBook)
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

        private List<DisplayBook> MapBooksToDisplayBooks(Author author, DisplayAuthor displayAuthor)
        {
            var result = new List<DisplayBook>();
            if(author.AuthorsBooks == null)
            {
                return result;
            }
            foreach (var authorBook in author.AuthorsBooks)
            {
                result.Add(new DisplayBook()
                {
                    Id = authorBook.BookId,
                    Title = authorBook.Book.Title
                });
            }
            return result;
        }
    }
}
