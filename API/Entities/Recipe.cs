using System;
using System.Collections.Generic;

namespace API.Entities
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PreparationTime { get; set; }
        public int NumberOfCalories { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public KitchenOrigin KitchenOrigin { get; set; }
        public int KitchenOriginId { get; set; }
        public ICollection<RecipePhoto> RecipePhotos { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}