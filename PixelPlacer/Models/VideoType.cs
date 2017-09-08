using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PixelPlacer.Models
{
    public class VideoType
    {
        [Key]
        public int VideoTypeId { get; set; }

        [Required]
        public string Category { get; set; }

        public IEnumerable<Video> Videos { get; set; }
    }
}
