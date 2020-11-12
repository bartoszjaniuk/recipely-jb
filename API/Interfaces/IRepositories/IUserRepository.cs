using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dto.Recipe;
using API.Dto.User;
using API.Entities;
using API.Helpers;

namespace API.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        void Update(AppUser appUser);
        void AddRecipeToFav (FavouriteRecipe favouriteRecipe);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);

        Task<MemberDto> GetMemberAsync(string username);
        Task <IEnumerable<FavouriteRecipeDto>> GetUserFavRecipes(int userId);
        Task<FavouriteRecipe> GetFav(int recipeId);
        
    }
}