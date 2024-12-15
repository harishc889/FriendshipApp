using FriendshipApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace FriendshipApp.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<AppUser> User { get; set; }
    }
}
