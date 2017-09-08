using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PixelPlacer.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required]
        [Display(Name = "Project Title")]
        [MaxLength(50), MinLength(1)]
        public string Title { get; set; }

        public virtual ApplicationUser User { get; set; }

        public ICollection<ProjectVideos> ProjectVideos { get; set; }
    }
}
