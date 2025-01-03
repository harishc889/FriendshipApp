using System.Security.Cryptography;
using System.Text;
using FriendshipApp.Data;
using FriendshipApp.DTOs;
using FriendshipApp.Entities;
using FriendshipApp.Interfaces;
using FriendshipApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FriendshipApp.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext dataContext, ITokenService tokenService)
        {
            _context = dataContext;
            _tokenService = tokenService;
        }

        [HttpPost("register")]  ///api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto register)
        {
            if (await IsUserExists(register.Username)) return BadRequest("Username already taken");
            
            var hash = new HMACSHA512();
            var user = new AppUser
            {
                UserName = register.Username.ToLower(),
                Password = hash.ComputeHash(Encoding.UTF8.GetBytes(register.Password)),
                PasswordSalt = hash.Key
            };
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

            return new UserDto { Username = user.UserName, Token = _tokenService.CreateToken(user) };
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            AppUser user = await _context.User.SingleOrDefaultAsync(u => u.UserName.ToLower().Equals(loginDto.Username.ToLower()));

            if (user == null) return Unauthorized("No user found");

            var hash = new HMACSHA512(user.PasswordSalt);
            var generatedPassword = hash.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            
            if (!generatedPassword.SequenceEqual(user.Password)) { return Unauthorized("Either Username or password not matched."); }

            return new UserDto { Username = user.UserName, Token = _tokenService.CreateToken(user)};
            


        }

        private async Task<bool> IsUserExists(string username) {
            return await _context.User.AnyAsync(user => user.UserName == username.ToLower());
        }
    }
}
