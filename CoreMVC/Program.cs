using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CoreMVC.Data;
using CoreMVC.Models;
using CoreMVC.Areas.Identity.Models;
using Pomelo.EntityFrameworkCore.MySql;
using CoreMvc.Api;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

//DB Context
builder.Services.AddDbContext<CoreMVCContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LIEN") ?? 
                    throw new InvalidOperationException("Connection string 'CoreMVCContext' not found.")));

builder.Services.AddDbContext<TestContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TEST") ?? 
                    throw new InvalidOperationException("Connection string 'TestContext' not found.")));

builder.Services.AddDbContext<MariaDBContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MariaDB") ?? 
                    throw new InvalidOperationException("Connection string 'MariaDBContext' not found."),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MariaDB"))));

// Add services to the container.
builder.Services.AddSingleton(builder.Configuration);
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

//Identity
builder.Services.AddIdentity<AccountModel, AccountRoleModel>(options =>
        options.SignIn.RequireConfirmedAccount = true) 
    .AddEntityFrameworkStores<MariaDBContext>()
    .AddDefaultTokenProviders();

//Password Rule
builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            }
        );

//Authorize Redirect
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});


//OAuth - Google
builder.Services.AddAuthentication(o =>
    {
        o.DefaultScheme = "Application";
        o.DefaultSignInScheme = "External";
    })
    .AddCookie("Application")
    .AddCookie("External")
    .AddGoogle(o =>
    {
        o.ClientId = builder.Configuration["AppSettings:GoogleOAuth:ClientId"];
        o.ClientSecret = builder.Configuration["AppSettings:GoogleOAuth:ClientSecret"];
    });




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

app.UseHttpsRedirection();
app.UseStaticFiles();

//授權
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.UseSession();
app.UseStaticFiles();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});




app.MapAreaControllerRoute(
        name: "Identity",
        areaName: "Identity",
        pattern: "Identity/{controller=Account}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
