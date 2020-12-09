using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dto.Recipe;
using API.Dto.User;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UsersController(IPhotoService photoService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _photoService = photoService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            userParams.CurrentUsername = user.UserName;

            var users = await _unitOfWork.UserRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCountInQuery, users.TotalNumberOfPages);

            return Ok(users);

        }
        [HttpGet("{username}", Name = "GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = await _unitOfWork.UserRepository.GetMemberAsync(username);
            return user;

        }

        [HttpGet("/api/User/favouriteRecipes")] // Pobieranie wartości
        public async Task<ActionResult> GetUserFavRecipes()
        {

            var userId = User.GetUserId();

            var userFavRecipes = await _unitOfWork.UserRepository.GetUserFavRecipes(userId);

            return Ok(userFavRecipes);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {


            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            _mapper.Map(memberUpdateDto, user);

            _unitOfWork.UserRepository.Update(user);

            if (await _unitOfWork.Complete()) return NoContent();
            return BadRequest("Failed to update user");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.UserPhotos.FirstOrDefault(p => p.Id == photoId);

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.UserPhotos.FirstOrDefault(p => p.IsMain);

            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to set main photo");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<UserPhotoDto>> AddPhoto(IFormFile file)
        {
            var username = User.GetUsername();

            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new UserPhoto
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            if (user.UserPhotos.Count == 0)
            {
                photo.IsMain = true;
            }
            user.UserPhotos.Add(photo);

            if (await _unitOfWork.Complete())
            {
                return CreatedAtRoute("GetUser", new { username = user.UserName }, _mapper.Map<UserPhotoDto>(photo));
            }
            return BadRequest("Problem adding photo");
        }


        [HttpPost("add-recipe")]
        public async Task<ActionResult> AddNewRecipe(RecipeForCreateDto recipeForCreateDto)
        {
            var userFromRepo = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            recipeForCreateDto.Name = recipeForCreateDto.Name.ToLower();

            if (await _unitOfWork.RecipeRepository.RecipeExists(recipeForCreateDto.Name))
                return BadRequest("Recipe with that name already exists!");

            var recipeToCreate = _mapper.Map<Recipe>(recipeForCreateDto);

            recipeToCreate.AppUserId = userFromRepo.Id;

            var createdRecipe = await _unitOfWork.RecipeRepository.AddNewRecipe(recipeToCreate);

            var recipeToReturn = _mapper.Map<RecipeForDetailDto>(createdRecipe);

            recipeToReturn.CategoryId = createdRecipe.CategoryId;
            recipeToReturn.KitchenOriginId = createdRecipe.KitchenOriginId;

            return CreatedAtRoute("GetRecipe", new { controller = "Recipes", id = createdRecipe.Id }, recipeToReturn);
        }



        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var username = User.GetUsername();
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

            var photo = user.UserPhotos.FirstOrDefault(p => p.Id == photoId);

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.UserPhotos.Remove(photo);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to delete the photo");
        }
    }
}