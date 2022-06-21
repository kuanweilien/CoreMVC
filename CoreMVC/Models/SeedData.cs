using CoreMVC.Data;
using Microsoft.EntityFrameworkCore;

namespace CoreMVC.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MariaDBContext(serviceProvider.GetRequiredService<DbContextOptions<MariaDBContext>>()))
            {
                // Look for any MovieModels.
                if (context.MovieModel.Any())
                {
                    return;   // DB has been seeded
                }

                context.MovieModel.AddRange(
                    new MovieModel
                    {
                        Title = "When Harry Met Sally",
                        ReleaseDate = DateTime.Parse("1989-2-12"),
                        Genre = "Romantic Comedy",
                        Rating = "R",
                        Price = 7.99M
                    },

                    new MovieModel
                    {
                        Title = "Ghostbusters ",
                        ReleaseDate = DateTime.Parse("1984-3-13"),
                        Genre = "Comedy",
                        Rating = "R",
                        Price = 8.99M
                    },

                    new MovieModel
                    {
                        Title = "Ghostbusters 2",
                        ReleaseDate = DateTime.Parse("1986-2-23"),
                        Genre = "Comedy",
                        Rating = "R",
                        Price = 9.99M
                    },

                    new MovieModel
                    {
                        Title = "Rio Bravo",
                        ReleaseDate = DateTime.Parse("1959-4-15"),
                        Genre = "Western",
                        Rating = "R",
                        Price = 3.99M
                    }
                );
                context.SaveChanges();
            }

            using (var context = new MariaDBContext(serviceProvider.GetRequiredService<DbContextOptions<MariaDBContext>>()))
            {
                if (context.StudentModel.Any())
                {
                    return;   // DB has been seeded
                }
                context.StudentModel.AddRange(
                   new StudentModel
                   {
                       StudentName = "Ben",
                       StudentNumber = 23,
                       Sex = 1
                   },
                   new StudentModel
                   {
                       StudentName = "Sandy",
                       StudentNumber = 24,
                       Sex = 0
                   }, 
                   new StudentModel
                   {
                       StudentName = "David",
                       StudentNumber = 25,
                       Sex = 1
                   }
                   );
                context.SaveChanges();
            }
        }
    }
}
