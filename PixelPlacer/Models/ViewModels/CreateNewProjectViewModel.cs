using PixelPlacer.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PixelPlacer.Models.ViewModels
{
    public class CreateNewProjectViewModel
    {
        //public Video Video { get; set; }

        //public Project Project { get; set; }

        public IEnumerable<Video> VideoList { get; set; }

        public CreateNewProjectViewModel(ApplicationDbContext context, ApplicationUser user, int id)
        {
            VideoList = context.Video.Where(v => v.User == user && v.VideoTypeId == id).ToList();
        }

        public CreateNewProjectViewModel(ApplicationDbContext contex, ApplicationUser user) { }
                
    }
}
