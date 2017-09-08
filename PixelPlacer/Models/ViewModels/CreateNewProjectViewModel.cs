using PixelPlacer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelPlacer.Models.ViewModels
{
    public class CreateNewProjectViewModel
    {
        public Video Video { get; set; }

        public IEnumerable<Video> VideoList { get; set; }

        public CreateNewProjectViewModel(ApplicationDbContext context, ApplicationUser user)
        {
            VideoList = context.Video.Where(v => v.User == user || v.IsStock == true).ToList();
        }
    }
}
