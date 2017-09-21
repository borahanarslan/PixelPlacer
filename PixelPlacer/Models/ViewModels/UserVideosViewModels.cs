using PixelPlacer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelPlacer.Models.ViewModels
{
    public class UserVideosViewModels
    {

        public IEnumerable<Video> Videos { get; set; }

        public UserVideosViewModels(ApplicationDbContext context, ApplicationUser user)
        {
            Videos = (from v in context.Video
                      where v.User == user
                      select v).ToList();
        }
    }
}
