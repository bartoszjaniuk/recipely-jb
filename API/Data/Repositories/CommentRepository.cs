using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dto;
using API.Entities;
using API.Interfaces.IRepositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _autoMapper;
        public CommentRepository(DataContext context, IMapper autoMapper)
        {
            _autoMapper = autoMapper;
            _context = context;
        }

        public void AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
        }



        public async Task<bool> DeleteComment(int commentId)
        {
            var comment = await _context.Comments.
            FirstOrDefaultAsync(e => e.Id == commentId);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                return true;
            }
            return false;
        }

        public async Task<CommentDto> GetComment(int id)
        {
            return await _context.Comments
            .Where(r => r.Id == id)
            .ProjectTo<CommentDto>(_autoMapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<CommentDto>> GetCommentsForRecipe(int recipeId)
        {
            return await _context.Comments
            .Where(r => r.Recipe.Id == recipeId)
            .ProjectTo<CommentDto>(_autoMapper.ConfigurationProvider)
           .ToListAsync();
        }
    }
}