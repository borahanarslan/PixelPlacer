using PixelPlacer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelPlacer.Models.ViewModels
{
    public class VideosInProjectViewModel
    {
        public IEnumerable<Video> VideosInProject { get; set; }

        public VideosInProjectViewModel(ApplicationDbContext context, ApplicationUser user)
        {
            //VideosInProject = (from v in context.Video
            //                   where v.User == user
            //                   join p in context.Project
            //                   on v.User equals p.User
            //                   join pv in context.ProjectVideos
            //                   on p.ProjectId equals pv.ProjectId
            //                   select v).ToList();
        }
    }
}
