using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CoreMVC.Data;
using CoreMVC.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CoreMVCContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LIEN") ?? throw new InvalidOperationException("Connection string 'CoreMVCContext' not found.")));

// Add services to the container.
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

    SeedData.Initialize(services);
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
