using System.Threading.Tasks;
using API.Interfaces.IRepositories;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository {get;}
        IMessageRepository MessageRepository {get;}
        ILikesRepository LikesRepository {get;}
        ICommentRepository CommentRepository {get;}
        IRecipeRepository RecipeRepository {get;}
        Task<bool> Complete ();
        bool HasChanges();
    }
}