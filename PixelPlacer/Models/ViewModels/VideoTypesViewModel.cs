using PixelPlacer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelPlacer.Models.ViewModels
{
    public class VideoTypesViewModel
    {
        public IEnumerable<VideoType> VideoTypes { get; set; }

        public VideoTypesViewModel(ApplicationDbContext context)
        {
            VideoTypes = context.VideoType.Where(vt => vt.VideoTypeId > 0).ToList();
        }
    }
}
