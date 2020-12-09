using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dto.Recipe;
using API.Dto.Recipe.RecipePhoto;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RecipesController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _unitOfWork;
        public RecipesController(IMapper mapper, IPhotoService photoService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _photoService = photoService;
            _mapper = mapper;
        }

        [HttpGet] // Pobieranie wartości
        public async Task<ActionResult<IEnumerable<RecipeForListDto>>> GetRecipes([FromQuery] RecipeParams recipeParams)
        {

            var recipes = await _unitOfWork.RecipeRepository.GetRecipesAsync(recipeParams);

            Response.AddPaginationHeader(recipes.CurrentPage, recipes.PageSize, recipes.TotalCountInQuery, recipes.TotalNumberOfPages);

            return Ok(recipes);
        }

        [HttpGet("{id}", Name = "GetRecipe")]
        public async Task<ActionResult<RecipeForDetailDto>> GetRecipe(int id)
        {
            var recipe = await _unitOfWork.RecipeRepository.GetRecipeAsync(id);
            return recipe;

        }


        [HttpGet("kitchen-origins")]
        public async Task<ActionResult> GetKitchenOrigins()
        {
            var kitchenOrigins = await _unitOfWork.RecipeRepository.GetKitchenOriginsWithRecipesAsync();
            return Ok(kitchenOrigins);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRecipe(int id)
        {
            var recipeFromRepo = await _unitOfWork.RecipeRepository.GetRecipeAsync(id);

            if (recipeFromRepo == null) return NotFound("Recipe with that id does not exist");

            await _unitOfWork.RecipeRepository.DeleteRecipe(id);

            if (await _unitOfWork.Complete())
                return Ok();

            return BadRequest("Failed to delete the recipe");
        }

        [HttpDelete("ingredients/{ingredientId}")]
        public async Task<ActionResult> DeleteIngredient(int ingredientId)
        {
            var ingredientFromRepo = await _unitOfWork.RecipeRepository.GetIngredient(ingredientId);

            if (ingredientFromRepo == null) return NotFound("Recipe with that id does not exist");

            await _unitOfWork.RecipeRepository.DeleteIngredient(ingredientId);

            if (await _unitOfWork.Complete())
                return Ok();

            return BadRequest("Failed to delete the ingredient");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRecipe(int id, RecipeForUpdateDto recipeForUpdateDto)
        {
            var recipeFromRepo = await _unitOfWork.RecipeRepository.GetRecipe(id);


            _mapper.Map(recipeForUpdateDto, recipeFromRepo);

            _unitOfWork.RecipeRepository.Update(recipeFromRepo);

            if (await _unitOfWork.Complete()) return NoContent();
            return BadRequest("Failed to update user");
        }

        [HttpPut("{recipeId}/set-main-photo/{id}")]
        public async Task<ActionResult> SetMainPhoto(int id, int recipeId)
        {

            var recipeFromRepo = await _unitOfWork.RecipeRepository.GetRecipe(recipeId);

            var photo = recipeFromRepo.RecipePhotos.FirstOrDefault(p => p.Id == id);

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = recipeFromRepo.RecipePhotos.FirstOrDefault(p => p.IsMain);

            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;


            if (await _unitOfWork.Complete())
            {
                return NoContent();
            }

            return BadRequest("Failed to set main photo");
        }



        [HttpPost("{id}/add-photo")]
        public async Task<ActionResult<RecipePhotoForDetailDto>> AddPhoto(IFormFile file, int id)
        {
            var recipe = await _unitOfWork.RecipeRepository.GetRecipe(id);

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

            if (await _unitOfWork.Complete())
            {
                return CreatedAtRoute("GetRecipe", new { id = recipe.Id }, _mapper.Map<RecipePhotoForDetailDto>(photo));
            }
            return BadRequest("Problem adding photo");
        }

        [HttpPost("{recipeId}/addToFav")]
        public async Task<ActionResult> AddRecipeToFav(int recipeId)
        {
            var currentUserId = 0;

            try
            {
                currentUserId = User.GetUserId();
            }
            catch (System.Exception)
            {

                return Unauthorized();
            }



            var user = _unitOfWork.UserRepository.GetUserByIdAsync(currentUserId);

            var like = await _unitOfWork.UserRepository.GetFav(recipeId);

            if (like != null)
                return BadRequest("You allready like this recipe");

            if (await _unitOfWork.RecipeRepository.GetRecipe(recipeId) == null)
                return NotFound();

            like = new FavouriteRecipe
            {
                UserId = currentUserId,
                RecipeId = recipeId
            };

            _unitOfWork.UserRepository.AddRecipeToFav(like);

            if (await _unitOfWork.Complete())
                return Ok();

            return BadRequest("Failed  to fav  recipe");
        }

        [HttpDelete("{id}/delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int id, int photoId)
        {
            var recipeFromRepo = await _unitOfWork.RecipeRepository.GetRecipe(id);

            var photo = recipeFromRepo.RecipePhotos.FirstOrDefault(p => p.Id == photoId);

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            recipeFromRepo.RecipePhotos.Remove(photo);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to delete the photo");
        }
    }




}