using System;

namespace API.Dto.Recipe.RecipePhoto
{
    public class RecipePhotoForDetailDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
    }
}