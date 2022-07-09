﻿using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CoreMVC.Models;
using CoreMVC.Areas.Identity.Models;
using CoreMVC.Models.User;
namespace CoreMVC.Data
{
    public class MariaDBContext : IdentityDbContext<AccountModel,AccountRoleModel,string>
    {
        public IConfiguration configuration;
        public MariaDBContext(DbContextOptions<MariaDBContext> options,IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;

        }
        
        public DbSet<CoreMVC.Models.MovieModel>? MovieModel { get; set; }
        
        public DbSet<CoreMVC.Models.StudentModel>? StudentModel { get; set; }
        
        public DbSet<CoreMVC.Models.PhotoModel>? PhotoModel { get; set; }
        public DbSet<CoreMVC.Models.User.UserModel>? UserModel { get; set; }
        public DbSet<CoreMVC.Models.User.RoleModel>? RoleModel { get; set; }
        public DbSet<CoreMVC.Models.User.GroupModel>? GroupModel { get; set; }
        public DbSet<CoreMVC.Models.User.UserGroupModel>? UserGroupModel { get; set; }
        public DbSet<CoreMVC.Areas.Identity.Models.AccountModel>? AccountModel { get; set; }
        public DbSet<CoreMVC.Areas.Identity.Models.AccountRoleModel>? AccountRoleModel { get; set; }
    }
}
