using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Interfaces.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _autoMapper;
        public UsersController(IUserRepository userRepository, IMapper autoMapper)
        {
            _autoMapper = autoMapper;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserToReturnDto>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();
            return Ok(users);

        }

        // [HttpGet("{id}")]
        // public async Task <ActionResult<AppUser>> GetUser(int id)
        // {
        //     var user = await _userRepository.GetUserByIdAsync(id);
        //     return Ok(user);

        // }

        [HttpGet("{username}")]
        public async Task<ActionResult<UserToReturnDto>> GetUser(string username)
        {
            var user = await _userRepository.GetMemberAsync(username);
            return user;

        }
    }
}