using System;
using System.Collections.Generic;
using API.Dto.Recipe.RecipePhoto;
using API.Entities;

namespace API.Dto.Recipe
{
    public class RecipeForDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PreparationTime { get; set; }
        public int NumberOfCalories { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId{ get; set; } 
        public int KitchenOriginId{ get; set; }
        public DateTime DateAdded { get; set; }
        public ICollection<RecipePhotoForDetailDto> RecipePhotos { get; set; }
        public ICollection<IngredientDto> Ingredients { get; set; }
        public ICollection<CommentDto> Comments { get; set; }

    }
}