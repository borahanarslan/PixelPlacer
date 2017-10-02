using Microsoft.AspNetCore.Mvc.Rendering;
using PixelPlacer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


// View Model used in Video/Edit View and in VideosController/Edit and SaveUploadChanges
// User can Update an Uploaded video Title and TypeID
namespace PixelPlacer.Models.ViewModels
{
    public class UpdateUploadViewModel
    {
        public Video Video { get; set; }

        public List<SelectListItem> VideoTypeList { get; set; }

        public UpdateUploadViewModel(ApplicationDbContext context, ApplicationUser user, int id)
        {
            Video = context.Video.SingleOrDefault(v => v.VideoId == id);

            this.VideoTypeList = context.VideoType
                       .OrderBy(v => v.Category)
                       .AsEnumerable()
                       .Select(vi => new SelectListItem
                       {
                           Text = vi.Category,
                           Value = vi.VideoTypeId.ToString()
                       }).ToList();
        }

        public UpdateUploadViewModel () { }

    }
}
