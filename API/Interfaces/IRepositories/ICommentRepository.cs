using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dto;
using API.Entities;

namespace API.Interfaces.IRepositories
{
    public interface ICommentRepository
    {
        void AddComment(Comment comment);
        Task<bool> DeleteComment(int commentId);
        Task<CommentDto> GetComment(int id);
        Task<IEnumerable<CommentDto>> GetCommentsForRecipe(int recipeId);
        Task<bool> SaveAllAsync();
    }
}