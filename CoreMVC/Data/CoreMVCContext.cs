using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoreMVC.Models;

namespace CoreMVC.Data
{
    public class CoreMVCContext : DbContext
    {
        public CoreMVCContext (DbContextOptions<CoreMVCContext> options)
            : base(options)
        {
        }

        public DbSet<CoreMVC.Models.MovieModel>? MovieModel { get; set; }
    }
}
