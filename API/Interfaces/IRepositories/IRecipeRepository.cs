using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dto.Recipe;
using API.Entities;

namespace API.Interfaces.IRepositories
{
    public interface IRecipeRepository
    {

        Task<bool> DeleteRecipe(int recipeId);
        Task<bool> DeleteIngredient(int ingredientId);
        void Update(Recipe recipe);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<RecipeForListDto>> GetRecipesAsync();
        Task<RecipeForDetailDto> GetRecipeAsync(int id);
        Task<Recipe> GetRecipe(int id);
        Task<Ingredient> GetIngredient(int id);
        Task<RecipeWithPhotosDto> GetRecipeWithPhotos(int id);
        Task<Recipe> AddNewRecipe(Recipe recipe);
        Task<bool> RecipeExists(string name);

        // Task<FavouriteRecipe> GetFav(int userId, int recipeId);
        // Task<RecipePhoto> GetRecipePhoto(int id);
        // Task<RecipePhoto> GetMainPhotoForRecipe(int recipeId);
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
        Task<CategoryDto> GetCategoryAsync(int id);

        Task<IEnumerable<KitchenOriginDto>> GetKitchenOriginsAsync();
        Task<KitchenOriginDto> GetKitchenOriginAsync(int id);
    }
}