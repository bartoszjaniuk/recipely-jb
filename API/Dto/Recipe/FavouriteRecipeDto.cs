namespace API.Dto.Recipe
{
    public class FavouriteRecipeDto
    {
        public int RecipeId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
    }
}