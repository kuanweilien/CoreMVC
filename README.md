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
