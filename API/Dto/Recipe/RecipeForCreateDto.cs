using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API.Dto.Recipe.RecipePhoto;

namespace API.Dto.Recipe
{
    public class RecipeForCreateDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "You must specify name between 3 and 50 characters")]
        public string Name { get; set; }

        [Required]
        public int PreparationTime { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int NumberOfCalories { get; set; }

        public int CategoryId { get; set; }

        public int KitchenOriginId { get; set; }

        [Required]
        public ICollection<IngredientDto> Ingredients { get; set; }
        public DateTime DateAdded { get; set; }
        public int AppUserId { get; set; }
        public int AuthorId { get; set; }

        public RecipeForCreateDto()
        {
            DateAdded = DateTime.Now;
        }
    }
}