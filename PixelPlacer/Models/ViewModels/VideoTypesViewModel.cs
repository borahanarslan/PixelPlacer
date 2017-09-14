using PixelPlacer.Classes;
using PixelPlacer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelPlacer.Models.ViewModels
{
    public class VideoTypesViewModel
    {
        public string ProjectTitle { get; set; }

        public List<VideoType> VideoTypes { get; set; }

        public List<BackGroundClass> BackGround { get; set; }

        public IEnumerable<SaveProjectClass> ProjectClass { get; set; }

        public List<OverlayClass> OverLay { get; set; }

        public VideoTypesViewModel(ApplicationDbContext context, ApplicationUser user)
        {
            ProjectClass = new List<SaveProjectClass>();

            VideoTypes = context.VideoType.Where(vt => vt.VideoTypeId > 0).ToList();

            BackGround = (from v in context.Video
                       where v.User == user 
                       join pv in context.ProjectVideos
                       on v.VideoId equals pv.VideoId
                       where pv.User == user
                       && pv.BackGround == true
                       join p in context.Project
                       on pv.ProjectId equals p.ProjectId
                       where p.Title == null
                       select new BackGroundClass
                       {
                           VTitle = v.VideoTitle,
                           VId = v.VideoId,
                           VFilePath = v.VideoFilePath,
                           VTypeId = v.VideoTypeId,
                           ProjId = pv.ProjectId,
                       }).ToList();

            if (BackGround.Count == 1)
            {
                VideoTypes.RemoveAt(0);
            }


            OverLay = (from v in context.Video
                       where v.User == user && v.VideoTypeId == 2
                       join pv in context.ProjectVideos
                       on v.VideoId equals pv.VideoId
                       where pv.User == user &&
                       pv.BackGround == false
                       join p in context.Project
                       on pv.ProjectId equals p.ProjectId
                       where p.Title == null
                       select new OverlayClass
                       {
                           VTitle = v.VideoTitle,
                           VId = v.VideoId,
                           VFilePath = v.VideoFilePath,
                           VTypeId = v.VideoTypeId,
                           ProjId = pv.ProjectId,
                           ProjVidId = pv.ProjectVideosId,
                           Thumb = v.Thumbnail
                       }).ToList();

            if (OverLay.Count >= 3 && BackGround.Count > 0)
            {
                VideoTypes.RemoveAt(0);

            } else if (BackGround.Count == 0 & OverLay.Count >= 3) {

                VideoTypes.RemoveAt(1);
            }

        }

        public VideoTypesViewModel() { }
    }
}

