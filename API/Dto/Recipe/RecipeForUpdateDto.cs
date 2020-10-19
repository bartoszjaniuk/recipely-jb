using System.Collections.Generic;

namespace API.Dto.Recipe
{
    public class RecipeForUpdateDto
    {
        public string Name { get; set; }
        public int PreparationTime { get; set; }
        public int NumberOfCalories { get; set; }
        public string Description { get; set; }
        public ICollection<IngredientDto> Ingredients { get; set; }
    }
}