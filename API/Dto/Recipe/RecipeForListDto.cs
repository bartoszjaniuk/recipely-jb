using System;
using System.Collections.Generic;

namespace API.Dto.Recipe
{
    public class RecipeForListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PreparationTime { get; set; }
        public int NumberOfCalories { get; set; }
        public string PhotoUrl { get; set; }
        public string Category { get; set; }
        public string KitchenOrigin { get; set; }
        public DateTime DateAdded { get; set; }
        public ICollection<IngredientDto> Ingredients { get; set; }
    }
}