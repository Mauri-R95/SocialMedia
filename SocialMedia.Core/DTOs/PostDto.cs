using System;

namespace SocialMedia.Core.DTOs
{
    public class PostDto
    {
        public int Id { get; set; }
        //public int PostId { get; set; }
        public int UserId { get; set; }
        public DateTime? Date { get; set; } // con el ? se vuelve Nulleable
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
