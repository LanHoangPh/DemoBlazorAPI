using Microsoft.EntityFrameworkCore;

namespace APIWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
       
    }
}
