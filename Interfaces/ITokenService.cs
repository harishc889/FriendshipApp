using FriendshipApp.Entities;

namespace FriendshipApp.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
