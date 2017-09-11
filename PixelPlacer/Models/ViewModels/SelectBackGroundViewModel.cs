using PixelPlacer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelPlacer.Models.ViewModels
{
    public class SelectBackGroundViewModel
    {
        public IEnumerable<Video> VideoList { get; set; }

        public SelectBackGroundViewModel(ApplicationDbContext context, ApplicationUser user)
        {
            VideoList = context.Video.Where(v => v.User == user || v.IsStock == true && v.VideoTitle == "BackGround").ToList();
        }
    }
}
