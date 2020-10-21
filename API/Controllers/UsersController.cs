using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dto.Recipe;
using API.Dto.User;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Interfaces.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _autoMapper;
        private readonly IPhotoService _photoService;
        private readonly IRecipeRepository _recipeRepository;
        public UsersController(IUserRepository userRepository, IMapper autoMapper, IPhotoService photoService, IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
            _photoService = photoService;
            _autoMapper = autoMapper;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();
            return Ok(users);

        }

        [HttpGet("{username}", Name = "GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = await _userRepository.GetMemberAsync(username);
            return user;

        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {


            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            _autoMapper.Map(memberUpdateDto, user);

            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update user");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.UserPhotos.FirstOrDefault(p => p.Id == photoId);

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.UserPhotos.FirstOrDefault(p => p.IsMain);

            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<UserPhotoDto>> AddPhoto(IFormFile file)
        {
            var username = User.GetUsername();

            var user = await _userRepository.GetUserByUsernameAsync(username);

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

            if (await _userRepository.SaveAllAsync())
            {
                return CreatedAtRoute("GetUser", new { username = user.UserName }, _autoMapper.Map<UserPhotoDto>(photo));
            }
            return BadRequest("Problem adding photo");
        }


        [HttpPost("add-recipe")]
        public async Task<ActionResult> AddNewRecipe(RecipeForCreateDto recipeForCreateDto)
        {
            var userFromRepo = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            recipeForCreateDto.Name = recipeForCreateDto.Name.ToLower();

            if (await _recipeRepository.RecipeExists(recipeForCreateDto.Name))
                return BadRequest("Recipe with that name already exists!");

            var recipeToCreate = _autoMapper.Map<Recipe>(recipeForCreateDto);

            recipeToCreate.AppUserId = userFromRepo.Id;

            var createdRecipe = await _recipeRepository.AddNewRecipe(recipeToCreate);

            var recipeToReturn = _autoMapper.Map<RecipeForDetailDto>(createdRecipe);

            recipeToReturn.CategoryId = createdRecipe.CategoryId;
            recipeToReturn.KitchenOriginId = createdRecipe.KitchenOriginId;

            return CreatedAtRoute("GetRecipe", new { controller = "Recipes", id = createdRecipe.Id}, recipeToReturn);
        }


        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var username = User.GetUsername();
            var user = await _userRepository.GetUserByUsernameAsync(username);

            var photo = user.UserPhotos.FirstOrDefault(p => p.Id == photoId);

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.UserPhotos.Remove(photo);

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete the photo");
        }
    }
}