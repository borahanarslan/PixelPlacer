﻿using System;
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
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public int VideoId { get; set; }
        public Video Video { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int XPosition { get; set; }

        public int YPosition { get; set; }

        public bool BackGround { get; set; }
    }
}
