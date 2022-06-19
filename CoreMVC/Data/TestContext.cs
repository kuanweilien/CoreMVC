using Microsoft.EntityFrameworkCore;

namespace CoreMVC.Data
{
    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions<TestContext> options)
            : base(options)
        {
        }
        public DbSet<CoreMVC.Models.MovieModel>? MovieModel { get; set; }
    }
}
