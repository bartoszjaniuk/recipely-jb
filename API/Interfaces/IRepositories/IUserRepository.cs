using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces.IRepositories
{
    public interface IUserRepository
    {
       void Update(AppUser appUser);
       Task<bool> SaveAllAsync();
       Task<IEnumerable<AppUser>> GetUsersAsync();
       Task<AppUser> GetUserByIdAsync(int id);
       Task<AppUser> GetUserByUsernameAsync(string username);

       Task<IEnumerable<UserToReturnDto>> GetMembersAsync();

       Task<UserToReturnDto> GetMemberAsync(string username);
    }
}