using PixelPlacer.Classes;
using PixelPlacer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelPlacer.Models.ViewModels
{
    public class ProjectPlayViewModel
    {
        public BackGroundClass BackGroundVideo { get; set; }

        public IEnumerable<OverlayClass> OverLayList { get; set; }

        public ProjectPlayViewModel(ApplicationDbContext context, ApplicationUser user, int id)
        {
            BackGroundVideo = (from v in context.Video
                               where v.User == user
                               join pv in context.ProjectVideos
                               on v.VideoId equals pv.VideoId
                               where pv.ProjectId == id &&
                               pv.BackGround == true
                               select new BackGroundClass
                               {
                                   VId = v.VideoId,
                                   VFilePath = v.VideoFilePath,
                                   VTypeId = v.VideoTypeId,
                                   ProjId = pv.ProjectId,
                                   ProjVidId = pv.ProjectVideosId,
                                   BackGround = pv.BackGround,
                                   XPosition = pv.XPosition,
                                   YPosition = pv.YPosition
                               }).SingleOrDefault();

            OverLayList = (from o in context.Video
                           where o.User == user
                           join pv in context.ProjectVideos
                           on o.VideoId equals pv.VideoId
                           where pv.ProjectId == id &&
                           pv.BackGround == false
                           select new OverlayClass
                           {
                               VId = o.VideoId,
                               VFilePath = o.VideoFilePath,
                               VTypeId = o.VideoTypeId,
                               ProjId = pv.ProjectId,
                               ProjVidId = pv.ProjectVideosId,
                               BackGround = pv.BackGround,
                               XPosition = pv.XPosition,
                               YPosition = pv.YPosition
                           }).ToList();
                         
                            
        }

    }
}
