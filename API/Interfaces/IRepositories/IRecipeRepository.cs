using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dto.Recipe;
using API.Entities;

namespace API.Interfaces.IRepositories
{
    public interface IRecipeRepository
    {
        
        Task<bool> DeleteRecipe(int recipeId);
        void Update(Recipe recipe);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<RecipeForListDto>> GetRecipesAsync();
        Task<RecipeForDetailDto> GetRecipeAsync(int id);
        Task<Recipe> AddNewRecipe(Recipe recipe);
        Task<bool> RecipeExists(string name);

        // Task<FavouriteRecipe> GetFav(int userId, int recipeId);
        // Task<RecipePhoto> GetRecipePhoto(int id);
        // Task<RecipePhoto> GetMainPhotoForRecipe(int recipeId);
        // Task<IEnumerable<Category>> GetCategories();
    }
}