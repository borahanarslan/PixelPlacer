using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PixelPlacer.Models
{
    public class ProjectVideos
    {
        [Key]
        public int ProjectVideosId { get; set; }

        [Required]
        public int SavedProjectId { get; set; }
        public Project Project { get; set; }

        [Required]
        public int VideoId { get; set; }
        public Video Video { get; set; }

        public int XPositition { get; set; }

        public int YPosition { get; set; }
    }
}
