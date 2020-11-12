using System;

namespace API.Entities
{
    public class FavouriteRecipe
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public DateTime DateLiked { get; set; }

        public FavouriteRecipe()
        {
            DateLiked = DateTime.Now;
        }
    }
}