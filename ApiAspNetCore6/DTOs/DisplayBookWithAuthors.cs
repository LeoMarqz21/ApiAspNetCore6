namespace ApiAspNetCore6.DTOs
{
    public class DisplayBookWithAuthors : DisplayBook
    {
        public List<DisplayAuthor> Authors { get; set; }
    }
}
