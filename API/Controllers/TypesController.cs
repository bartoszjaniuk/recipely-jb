using System.Threading.Tasks;
using API.Interfaces;
using API.Interfaces.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TypesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public TypesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        [HttpGet("categories")]
        public async Task<ActionResult> GetCategories()
        {
            var categories = await _unitOfWork.RecipeRepository.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("categories/{id}")]
        public async Task<ActionResult> GetCategories(int id)
        {
            var category = await _unitOfWork.RecipeRepository.GetCategoryAsync(id);
            return Ok(category);
        }

        [HttpGet("kitchen-origins")]
        public async Task<ActionResult> GetKitchenOrigins()
        {
            var kitchenOrigins = await _unitOfWork.RecipeRepository.GetKitchenOriginsAsync();
            return Ok(kitchenOrigins);
        }

        [HttpGet("kitchen-origins/{id}")]
        public async Task<ActionResult> GetKitchenOrigin(int id)
        {
            var kitchenOrigin = await _unitOfWork.RecipeRepository.GetKitchenOriginAsync(id);
            return Ok(kitchenOrigin);
        }




    }
}