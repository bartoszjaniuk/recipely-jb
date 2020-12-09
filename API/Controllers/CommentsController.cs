using System;
using System.Threading.Tasks;
using API.Dto;
using API.Dto.Comment;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class CommentsController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public CommentsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [Authorize]
        [HttpPost("/api/Recipes/{recipeId}/[controller]")]
        public async Task<ActionResult<CommentDto>> CreateComment(CommentToCreateDto commentToCreateDto, int recipeId)
        {
            var recipe = await _unitOfWork.RecipeRepository.GetRecipe(recipeId);

            if (recipe == null) return NotFound();

            var user = User.GetUsername();

            var author = await _unitOfWork.UserRepository.GetUserByUsernameAsync(user);

            var comment = new Comment
            {
                Author = author,
                Recipe = recipe,
                Body = commentToCreateDto.Body,
                CreatedAt = DateTime.Now
            };
            _unitOfWork.CommentRepository.AddComment(comment);

            if (await _unitOfWork.Complete()) return Ok(_mapper.Map<CommentDto>(comment));

            return BadRequest("Failed to send message");

        }

        [HttpGet("/api/Recipes/{recipeId}/[controller]")]
        public async Task<ActionResult> GetRecipes(int recipeId)
        {
            var comments = await _unitOfWork.CommentRepository.GetCommentsForRecipe(recipeId);
            return Ok(comments);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(int id)
        {
            var username = User.GetUsername();

            var comment = await _unitOfWork.CommentRepository.GetComment(id);

            if (comment.Username != username) return Unauthorized();

            await _unitOfWork.CommentRepository.DeleteComment(id);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest();

        }
    }
}