using System.Threading.Tasks;
using API.Interfaces.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TypesController : BaseApiController
    {
        private readonly IRecipeRepository _recipeRepository;
        public TypesController(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        [HttpGet("categories")]
        public async Task<ActionResult> GetCategories()
        {
            var categories = await _recipeRepository.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("categories/{id}")]
        public async Task<ActionResult> GetCategories(int id)
        {
            var category = await _recipeRepository.GetCategoryAsync(id);
            return Ok(category);
        }

         [HttpGet("kitchen-origins")]
        public async Task<ActionResult> GetKitchenOrigins()
        {
            var kitchenOrigins = await _recipeRepository.GetKitchenOriginsAsync();
            return Ok(kitchenOrigins);
        }

        [HttpGet("kitchen-origins/{id}")]
        public async Task<ActionResult> GetKitchenOrigin(int id)
        {
            var kitchenOrigin = await _recipeRepository.GetKitchenOriginAsync(id);
            return Ok(kitchenOrigin);
        }




    }
}