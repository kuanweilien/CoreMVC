using CoreMVC.Data;
using CoreMVC.Areas.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CoreMVC.Areas.Shop.Models;

namespace CoreMVC.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MariaDBContext(serviceProvider.GetRequiredService<DbContextOptions<MariaDBContext>>()))
            {
                // Look for any MovieModels.
                if (!context.MovieModel.Any())
                {
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
            }

            using (var context = new MariaDBContext(serviceProvider.GetRequiredService<DbContextOptions<MariaDBContext>>()))
            {
                if (!context.StudentModel.Any())
                {
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

            using (var context = new MariaDBContext(serviceProvider.GetRequiredService<DbContextOptions<MariaDBContext>>()))
            {
                if (!context.ItemModel.Any())
                {
                    context.ItemModel.AddRange(
                    new ItemModel() { Name = "iPhone X", CreationDate = DateTime.Now },
                    new ItemModel() { Name = "Zenfone 3", CreationDate = DateTime.Now },
                    new ItemModel() { Name = "Galaxy A52S", CreationDate = DateTime.Now }
                   );
                   context.SaveChanges();
                }
                
            }

            
        }
        public static async Task<string> DefaultAccount(IServiceProvider serviceProvider)
        {
            IConfiguration config = serviceProvider.GetRequiredService<IConfiguration>();

            using (var roleManager = serviceProvider.GetRequiredService<RoleManager<AccountRoleModel>>())
            {
                if (roleManager.Roles.Count() == 0)
                {
                    IConfigurationSection roles = config.GetSection("AppSettings:DefaultAccount:Roles");
                    foreach (KeyValuePair<string,string> roleName in roles.AsEnumerable())
                    {
                        AccountRoleModel role = new AccountRoleModel();
                        role.CreationDate = DateTime.Now;
                        role.UpdateDate = DateTime.Now;
                        role.Name = roleName.Value;
                        await roleManager.CreateAsync(role);
                    }
                    
                }
            }

            using (var userManager = serviceProvider.GetRequiredService<UserManager<AccountModel>>())
            {

                if (userManager.Users.Count() == 0)
                {
                    AccountModel account = new AccountModel();
                    account.Email = config["AppSettings:DefaultAccount:Email"];
                    account.UserName = config["AppSettings:DefaultAccount:Account"];
                    account.CreationDate = DateTime.Now;
                    account.UpdateDate = DateTime.Now;
                    await userManager.CreateAsync(account, config["AppSettings:DefaultAccount:Password"]);
                }
                AccountModel admin = await userManager.FindByEmailAsync(config["AppSettings:DefaultAccount:Email"]);
                await userManager.AddToRoleAsync(admin, config["AppSettings:DefaultAccount:Roles:0"]);
            }
            return string.Empty;
        }
    }
}
