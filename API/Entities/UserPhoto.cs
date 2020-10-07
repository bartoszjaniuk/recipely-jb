using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("UserPhotos")]
    public class UserPhoto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        // public string Description { get; set; }
        // public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
    }
}