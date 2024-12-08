using Microsoft.EntityFrameworkCore;
using RegisterLogin.Models;

namespace RegisterLogin.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>op):base(op) { }
        public DbSet<Users> user { get; set; }

    }
}
