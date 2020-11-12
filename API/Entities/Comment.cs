using System;

namespace API.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public AppUser Author { get; set; }
        public Recipe Recipe { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}