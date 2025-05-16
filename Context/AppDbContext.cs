using DON.Models;
using Microsoft.EntityFrameworkCore;

namespace DON.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
    
}
}
