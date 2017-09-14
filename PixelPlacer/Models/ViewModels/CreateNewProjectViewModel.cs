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
        public Video Video { get; set; }

        public Project Project { get; set; }

        // List to Hold Videos that a User can Select for a BackGround
        // this list is used on AddBackGroundVideo.cshtml in Projects
        // and is given a contraint of 1 video
        public IEnumerable<Video> VideoList { get; set; }

        // List to hold videos that a User can Select for OverLay
        // this List is used on AddOverLayVideo.cshtml and if given a 
        // contraint of 3
        public IEnumerable<Video> OverLayList { get; set; }

        public CreateNewProjectViewModel(ApplicationDbContext context, ApplicationUser user)
        {
            VideoList = context.Video.Where(v => v.User == user).ToList();

            OverLayList = context.Video.Where(v => v.User == user && v.VideoTypeId == 2).ToList();
        }               
    }
}
