using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PixelPlacer.Models
{
    public class Video
    {
        [Key]
        public int VideoId { get; set; }

        [Required]
        [Display(Name = "Video Title")]
        public string VideoTitle { get; set; }

        [Required]
        [Display(Name = "Upload")]
        public string VideoFilePath { get; set; }

        public string Thumbnail { get; set; }

        public bool IsStock { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }

        [Required]
        [Display(Name = "Video Type")]
        public int? VideoTypeId { get; set; }
        public VideoType VideoType { get; set; }
    }
}
