using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dto.User;
using API.Entities;

namespace API.Interfaces.IRepositories
{
    public interface IUserRepository
    {

        void Add<T>(T entity) where T : class;
        // void Delete<T>(T entity) where T : class;

        void Update(AppUser appUser);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);

        Task<IEnumerable<MemberDto>> GetMembersAsync();

        Task<MemberDto> GetMemberAsync(string username);
    }
}