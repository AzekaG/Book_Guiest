using Microsoft.EntityFrameworkCore;

namespace Book_Guiest.Models
{
    public class UsersContext: DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public UsersContext(DbContextOptions<UsersContext> options) : base(options)
        {
            Database.EnsureCreated();
        }


    }
}
