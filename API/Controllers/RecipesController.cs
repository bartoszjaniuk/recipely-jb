using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dto.Recipe;
using API.Interfaces.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RecipesController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IRecipeRepository _recipeRepository;
        public RecipesController(IRecipeRepository recipeRepository, IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        [HttpGet] // Pobieranie wartości
        public async Task<ActionResult<IEnumerable<RecipeForListDto>>> GetRecipes()
        {

            var recipes = await _recipeRepository.GetRecipesAsync();

            var recipesToReturn = _mapper.Map<IEnumerable<RecipeForListDto>>(recipes);

            return Ok(recipesToReturn);
        }

        [HttpGet("{id}", Name = "GetRecipe")]
        public async Task<ActionResult<RecipeForDetailDto>> GetRecipe(int id)
        {
            var recipe = await _recipeRepository.GetRecipeAsync(id);
            return recipe;

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRecipe(int id)
        {
            var recipeFromRepo = await _recipeRepository.GetRecipeAsync(id);

            if(recipeFromRepo == null) return NotFound("Recipe with that id does not exist");

            await _recipeRepository.DeleteRecipe(id);

            if (await _recipeRepository.SaveAllAsync())
                return Ok();

            return BadRequest("Failed to delete the recipe");
        }
    }

}