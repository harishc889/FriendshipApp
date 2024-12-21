using FriendshipApp.Data;
using FriendshipApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FriendshipApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //api/users
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context; 
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return await _context.User.ToListAsync();
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return  await _context.User.FindAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<AppUser>> InsertUser([FromBody] AppUser appUser)
        {
            if (appUser == null) { return NoContent(); }

            _context.User.Add(appUser);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
