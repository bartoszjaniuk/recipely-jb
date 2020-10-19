using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dto.Recipe;
using API.Entities;
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
            return Ok(recipes);
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

            if (recipeFromRepo == null) return NotFound("Recipe with that id does not exist");

            await _recipeRepository.DeleteRecipe(id);

            if (await _recipeRepository.SaveAllAsync())
                return Ok();

            return BadRequest("Failed to delete the recipe");
        }

        [HttpPut("{id}")] 
        public async Task<ActionResult> UpdateRecipe(int id, RecipeForUpdateDto recipeForUpdateDto)
        {
            var recipeFromRepo = await _recipeRepository.GetRecipe(id);

            
             _mapper.Map(recipeForUpdateDto, recipeFromRepo);

            _recipeRepository.Update(recipeFromRepo);

            if (await _recipeRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update user");
        }

        
        // TODO
        // addPhoto
        // setMainPhoto
        // deletePhoto
    }

}