using Microsoft.EntityFrameworkCore;

namespace LoginReg.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        
        // this is the variable we will use to connect to the MySQL table, Lizards
        public DbSet<User> Users { get; set; }
    }
}