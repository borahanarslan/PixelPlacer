using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PixelPlacer.Models;

namespace PixelPlacer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<PixelPlacer.Models.Project> Project { get; set; }

        public DbSet<PixelPlacer.Models.ProjectVideos> ProjectVideos { get; set; }

        public DbSet<PixelPlacer.Models.Video> Video { get; set; }

        public DbSet<PixelPlacer.Models.VideoType> VideoType { get; set; }
    }
}
