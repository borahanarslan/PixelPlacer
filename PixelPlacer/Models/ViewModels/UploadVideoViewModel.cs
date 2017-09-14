using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PixelPlacer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// View Model used in VideosController for Upload Action
// used in Upload.cshtml 
namespace PixelPlacer.Models.ViewModels
{
    public class UploadVideoViewModel
    {
        public Video Video { get; set; }

        public IFormFile UserVideo { get; set; }

        public List<SelectListItem> VideoTypeList { get; set; }

        public UploadVideoViewModel(ApplicationDbContext context)
        {
            // create a drop down that displays video types
            Video = new Video();
            this.VideoTypeList = context.VideoType
                               .OrderBy(v => v.Category)
                               .AsEnumerable()
                               .Select(vi => new SelectListItem
                               {
                                   Text = vi.Category,
                                   Value = vi.VideoTypeId.ToString()
                               }).ToList();
            this.VideoTypeList.Insert(0, new SelectListItem
            {
                Text = "Select Video Category",
                Value = "0"
            });
        }
        public UploadVideoViewModel() { }
    }
}
