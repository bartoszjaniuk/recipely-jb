using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("RecipePhotos")]
    public class RecipePhoto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public Recipe Recipe { get; set; }
        public int RecipeId { get; set; }
    }
}