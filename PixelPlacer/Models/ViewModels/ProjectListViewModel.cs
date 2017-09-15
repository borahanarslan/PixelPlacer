using PixelPlacer.Classes;
using PixelPlacer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelPlacer.Models.ViewModels
{
    public class ProjectListViewModel
    {

        public IEnumerable<Project> Projects { get; set; }

        public ProjectListViewModel(ApplicationDbContext context, ApplicationUser user)
        {
            Projects = context.Project.Where(video => video.User == user && video.Title != null).ToList();
        }
    }
}


