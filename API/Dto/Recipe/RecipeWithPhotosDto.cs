using System.Collections.Generic;
using API.Dto.Recipe.RecipePhoto;

namespace API.Dto.Recipe
{
    public class RecipeWithPhotosDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RecipePhotoForDetailDto> RecipePhotos { get; set; }
    }
}