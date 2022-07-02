using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CoreMVC.Data;
using CoreMVC.Models;
using Pomelo.EntityFrameworkCore.MySql;
using CoreMvc.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CoreMVCContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LIEN") ?? throw new InvalidOperationException("Connection string 'CoreMVCContext' not found.")));

builder.Services.AddDbContext<TestContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TEST") ?? throw new InvalidOperationException("Connection string 'TestContext' not found.")));

builder.Services.AddDbContext<MariaDBContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MariaDB") ?? 
                    throw new InvalidOperationException("Connection string 'MariaDBContext' not found."),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MariaDB"))));

// Add services to the container.
builder.Services.AddSingleton(builder.Configuration);
builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services,builder.Configuration);
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
