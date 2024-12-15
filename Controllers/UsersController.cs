using FriendshipApp.Data;
using FriendshipApp.Entities;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<IEnumerable<AppUser>> GetUsers()
        {
            return _context.User.ToList();
        }

        [HttpGet("{id}")]

        public ActionResult<AppUser> GetUser(int id)
        {
            return _context.User.Find(id);
        }
    }
}
