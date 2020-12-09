using System;
using System.Threading.Tasks;
using API.Dto;
using API.Dto.Comment;
using API.Entities;
using API.Extensions;
using API.Interfaces.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class CommentsController : BaseApiController
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;
        public CommentsController(IUserRepository userRepository, ICommentRepository commentRepository,
        IRecipeRepository recipeRepository, IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
        }
        [Authorize]
        [HttpPost("/api/Recipes/{recipeId}/[controller]")]
        public async Task<ActionResult<CommentDto>> CreateComment(CommentToCreateDto commentToCreateDto, int recipeId)
        {
            var recipe = await _recipeRepository.GetRecipe(recipeId);

            if (recipe == null) return NotFound();

            var user = User.GetUsername();

            var author = await _userRepository.GetUserByUsernameAsync(user);

            var comment = new Comment
            {
                Author = author,
                Recipe = recipe,
                Body = commentToCreateDto.Body,
                CreatedAt = DateTime.Now
            };
            _commentRepository.AddComment(comment);

            if (await _commentRepository.SaveAllAsync()) return Ok(_mapper.Map<CommentDto>(comment));

            return BadRequest("Failed to send message");

        }

        [HttpGet("/api/Recipes/{recipeId}/[controller]")]
        public async Task<ActionResult> GetRecipes(int recipeId)
        {
            var comments = await _commentRepository.GetCommentsForRecipe(recipeId);
            return Ok(comments);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(int id)
        {
            var username = User.GetUsername();

            var comment = await _commentRepository.GetComment(id);

            if (comment.Username != username) return Unauthorized();

            await _commentRepository.DeleteComment(id);

            if (await _commentRepository.SaveAllAsync()) return Ok();

            return BadRequest();

        }
    }
}