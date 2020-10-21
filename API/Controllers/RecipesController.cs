using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dto.Recipe;
using API.Dto.Recipe.RecipePhoto;
using API.Entities;
using API.Interfaces;
using API.Interfaces.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RecipesController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IPhotoService _photoService;
        public RecipesController(IRecipeRepository recipeRepository, IMapper mapper, IPhotoService photoService)
        {
            _photoService = photoService;
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

        [HttpPut("{recipeId}/set-main-photo/{id}")]
        public async Task<ActionResult> SetMainPhoto(int id, int recipeId)
        {

            var recipeFromRepo = await _recipeRepository.GetRecipe(recipeId);

            var photo = recipeFromRepo.RecipePhotos.FirstOrDefault(p => p.Id == id);

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = recipeFromRepo.RecipePhotos.FirstOrDefault(p => p.IsMain);

            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;
            

            if (await _recipeRepository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Failed to set main photo");
        }

        

        [HttpPost("{id}/add-photo")]
        public async Task<ActionResult<RecipePhotoForDetailDto>> AddPhoto(IFormFile file, int id)
        {    
            var recipe = await _recipeRepository.GetRecipe(id);

            var result = await _photoService.AddPhotoAsync(file);


            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new RecipePhoto
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            if (recipe.RecipePhotos.Count == 0)
            {
                photo.IsMain = true;
            }
            recipe.RecipePhotos.Add(photo);

            if (await _recipeRepository.SaveAllAsync())
            {
                return CreatedAtRoute("GetRecipe", new { id = recipe.Id }, _mapper.Map<RecipePhotoForDetailDto>(photo));
            }
            return BadRequest("Problem adding photo");
        }

        [HttpDelete("{id}/delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int id, int photoId)
        {
            var recipeFromRepo = await _recipeRepository.GetRecipe(id);

            var photo = recipeFromRepo.RecipePhotos.FirstOrDefault(p => p.Id == photoId);

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            recipeFromRepo.RecipePhotos.Remove(photo);

            if (await _recipeRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete the photo");
        }
    }

}