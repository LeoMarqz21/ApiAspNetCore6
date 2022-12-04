using ApiAspNetCore6.Entities;

namespace ApiAspNetCore6.DTOs
{
    public class DisplayBook
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
