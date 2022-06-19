using Microsoft.EntityFrameworkCore;

namespace CoreMVC.Data
{
    public class MariaDBContext : DbContext
    {
        
        public MariaDBContext(DbContextOptions<MariaDBContext> options) : base(options)
        {
                        

        }
        
        public DbSet<CoreMVC.Models.MovieModel>? MovieModel { get; set; }
    }
}
