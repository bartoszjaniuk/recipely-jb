using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dto.Recipe;
using API.Entities;
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

        public async Task<IEnumerable<RecipeForListDto>> GetRecipesAsync()
        {
            return await _context.Recipes
            .ProjectTo<RecipeForListDto>(_autoMapper.ConfigurationProvider)
            .ToListAsync();
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
    }
}