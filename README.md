# CoreMVC
### .Net Core 6.0 ASP.NET MVC 
* Document : https://docs.microsoft.com/zh-tw/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-6.0&tabs=visual-studio
---
### Deploy MVC Project to IIS 
* IIS Error :  500.19 0x8007000d - Install Windows Server Hosting
---
### Migration 
* Add Migration
```
 Add-Migration {Migration Name} 
```
* Update to Database
```
Update-Database
```

### Migration - Multiple Database
* Add Connection String on appsettings.json
* Add DBContext under /Data
* Add Migration Folder under /Migrations
* Add DBContext on Program.cs
```cs
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<{Context Name}>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("{Config ConnectionStrings Key}"));
```
* Enable Migration
```
Enable-migrations -ContextTypeName {Context Name} -MigrationDirectory Migrations.Log Add the migration
```
* Add Migration 
```
Add-Migration {Migration Name} -Context {Context Name} -OutputDir Migrations\{Folder Name}
```
* Update to Database
```
Update-Database -Context {Context Name} 
```
### Migration - MySql/MariaDB
* Install Package
```
Install-Package Pomelo.EntityFrameworkCore.MySql
```
https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql
* Add DBContext on Program.cs
```cs
builder.Services.AddDbContext<{Context Name}>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("{Config ConnectionStrings Key}"),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("{Config ConnectionStrings Key}"))));
```
---
### ASP.NET - Identity
* Install Package
```
Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore
```
* Modify DB Context
```cs
public class YourDBContext : IdentityDbContext<AccountModel,AccountRoleModel,string>
```
* IdentityRole Customerlize
```cs
public class NewUserModel:IdentityUser
{
  [PersonalData]
  public string NewColumn { get; set; }
}
```
* IdentityRole Customerlize
```cs
public class NewRoleModel:IdentityRole
{
  [PersonalData]
  public string NewColumn { get; set; }
}
```
* Register Program.cs
```cs
builder.Services.AddIdentity<NewUserModel, NewRoleModel>(options =>
        options.SignIn.RequireConfirmedAccount = true) 
    .AddEntityFrameworkStores<YourDBContext>()
    .AddDefaultTokenProviders();
```
* Config Program.cs
```cs
builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                ...
            }
        );
```
https://docs.microsoft.com/zh-tw/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-6.0
* inject view
```
@using Microsoft.AspNetCore.Identity
@inject SignInManager<NewUserModel> SignInManager
@inject UserManager<NewUserModel> UserManager
```

---
### ASP.NET - Area
* Add Folder
```
  Project
    └ Area
      └ Area1
         ├ Controllers
         ├ Models
         └ Views
            ├ Shared
            ├ _ViewImports.cshtml
            └ _ViewStart.cshtml
      
```
* Modify Program.cs
```cs
app.MapAreaControllerRoute(
        name: "Identity",      //Area Name
        areaName: "Identity",  //Area Name
        pattern: "Identity/{controller=Account}/{action=Index}/{id?}");
```
