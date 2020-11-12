using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dto.Recipe;
using API.Dto.User;
using API.Entities;
using API.Helpers;
using API.Interfaces.IRepositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _autoMapper;
        public UserRepository(DataContext context, IMapper autoMapper)
        {
            _autoMapper = autoMapper;
            _context = context;
        }



        public void AddRecipeToFav(FavouriteRecipe favouriteRecipe)
        {
            _context.FavouriteRecipes.Add(favouriteRecipe);
        }

        public async Task<FavouriteRecipe> GetFav(int recipeId)
        {
            return await _context.FavouriteRecipes.FirstOrDefaultAsync(u => u.RecipeId == recipeId);
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
            .Where(u => u.UserName == username)
            .ProjectTo<MemberDto>(_autoMapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();


            query = query.Where(u => u.UserName != userParams.CurrentUsername);

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created), // break is no need anymore
                _ => query.OrderByDescending(u => u.LastActive) // default
            };

            return await PagedList<MemberDto>
            .CreateAsync(query
            .ProjectTo<MemberDto>(_autoMapper.ConfigurationProvider)
            .AsNoTracking(), userParams.PageNumber, userParams.PageSize);

        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users
            .Where(u => u.Id == id)
            .Include(u => u.FavRecipes)
            .SingleOrDefaultAsync();
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
            .Include(u => u.UserPhotos)
            .SingleOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<IEnumerable<FavouriteRecipeDto>> GetUserFavRecipes(int userId)
        {
            return await _context.FavouriteRecipes
            .Where(x => x.UserId == userId)
            .ProjectTo<FavouriteRecipeDto>(_autoMapper.ConfigurationProvider)
            .ToListAsync();
            

            
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
            .Include(u => u.UserPhotos)
            .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser appUser)
        {
            _context.Entry(appUser).State = EntityState.Modified;
        }
    }
}