using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dto.Recipe;
using API.Entities;
using API.Helpers;
using API.Interfaces.IRepositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _autoMapper;
        public RecipeRepository(DataContext context, IMapper autoMapper)
        {
            _autoMapper = autoMapper;
            _context = context;
        }

        public async Task<RecipeForDetailDto> GetRecipeAsync(int id)
        {
            return await _context.Recipes.Where(r => r.Id == id)
            .ProjectTo<RecipeForDetailDto>(_autoMapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        }

        public async Task<RecipeWithPhotosDto> GetRecipeWithPhotos(int id)
        {
            return await _context.Recipes.Where(r => r.Id == id)
            .ProjectTo<RecipeWithPhotosDto>(_autoMapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        }

        public async Task<PagedList<RecipeForListDto>> GetRecipesAsync(RecipeParams recipeParams)
        {
            var query = _context.Recipes.AsQueryable();

            if (!string.IsNullOrEmpty(recipeParams.KitchenOriginId.ToString()))
            {
                query = query.Where(u => u.KitchenOrigin.Id == recipeParams.KitchenOriginId);
            }

            if (!string.IsNullOrEmpty(recipeParams.CategoryId.ToString()))
            {
                query = query.Where(u => u.Category.Id == recipeParams.CategoryId);
            }


            if (!string.IsNullOrEmpty(recipeParams.Search))
            {
                query = query.Where(s => s.Name.Contains(recipeParams.Search)
                                       || s.Category.Name.Contains(recipeParams.Search)
                                       || s.Ingredients.Select(s=> s.Name).Contains(recipeParams.Search)
                                       );
            }

            query = recipeParams.OrderBy switch
            {
                "maxTime" => query.OrderByDescending(u => u.PreparationTime), // break is no need anymore
                "minTime" => query.OrderBy(u => u.PreparationTime),
                _ => query.OrderBy(u => u.Name)
            };

            return await PagedList<RecipeForListDto>
            .CreateAsync(query
            .ProjectTo<RecipeForListDto>(_autoMapper.ConfigurationProvider)
            .AsNoTracking(), recipeParams.PageNumber, recipeParams.PageSize);

        }





        public async Task<bool> RecipeExists(string name)
        {
            if (await _context.Recipes.AnyAsync(r => r.Name == name))
                return true;
            return false;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Recipe recipe)
        {
            _context.Entry(recipe).State = EntityState.Modified;
        }

        public async Task<Recipe> AddNewRecipe(Recipe recipe)
        {
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();

            return recipe;
        }
        // TEMP
        public async Task<bool> DeleteRecipe(int recipeId)
        {
            var recipe = await _context.Recipes.
            FirstOrDefaultAsync(e => e.Id == recipeId);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
                return true;
            }
            return false;
        }

        public async Task<Recipe> GetRecipe(int id)
        {
            return await _context.Recipes
           .Include(u => u.RecipePhotos)
           .SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            return await _context.Categories
           .ProjectTo<CategoryDto>(_autoMapper.ConfigurationProvider)
           .ToListAsync();
        }

        public async Task<CategoryDto> GetCategoryAsync(int id)
        {
            return await _context.Categories.Where(r => r.Id == id)
           .ProjectTo<CategoryDto>(_autoMapper.ConfigurationProvider)
           .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<KitchenOriginDto>> GetKitchenOriginsAsync()
        {
            return await _context.KitchenOrigins
           .ProjectTo<KitchenOriginDto>(_autoMapper.ConfigurationProvider)
           .ToListAsync();
        }

        public async Task<KitchenOriginDto> GetKitchenOriginAsync(int id)
        {
            return await _context.KitchenOrigins.Where(r => r.Id == id)
           .ProjectTo<KitchenOriginDto>(_autoMapper.ConfigurationProvider)
           .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteIngredient(int ingredientId)
        {
            var ingredient = await _context.Ingredients.FirstOrDefaultAsync(e => e.Id == ingredientId);
            if (ingredient != null)
            {
                _context.Ingredients.Remove(ingredient);
                return true;
            }
            return false;
        }

        public async Task<Ingredient> GetIngredient(int id)
        {
            return await _context.Ingredients.SingleOrDefaultAsync(u => u.Id == id);
        }
    }
}