using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PixelPlacer.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

// View Model used in VideosController for Upload Action
// used in Upload.cshtml 
namespace PixelPlacer.Models.ViewModels
{
    public class UploadVideoViewModel
    {
        public Video Video { get; set; }
        public VideoType VT { get; set; }

        [Required]
        public IFormFile UserVideo { get; set; }

        public List<SelectListItem> VideoTypeList { get; set; }

        public UploadVideoViewModel(ApplicationDbContext context)
        {

            Video = new Video();

            // create a drop down that displays video types
            this.VideoTypeList = context.VideoType
                               .OrderBy(v => v.Category)
                               .AsEnumerable()
                               .Select(vi => new SelectListItem
                               {
                                   Text = vi.Category,
                                   Value = vi.VideoTypeId.ToString()
                               }).ToList();
        }
        public UploadVideoViewModel() { }
    }
}
