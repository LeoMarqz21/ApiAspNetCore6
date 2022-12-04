namespace ApiAspNetCore6.DTOs
{
    public class DisplayAuthorWithBooks : DisplayAuthor
    {
        public List<DisplayBook> Books { get; set; }
    }
}
