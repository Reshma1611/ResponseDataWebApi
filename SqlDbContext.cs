using Microsoft.EntityFrameworkCore;

namespace ResponseDataWebAPI
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext(DbContextOptions option) : base(option)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
