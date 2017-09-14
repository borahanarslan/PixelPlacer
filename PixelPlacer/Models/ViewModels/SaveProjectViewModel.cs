using PixelPlacer.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelPlacer.Models.ViewModels
{
    public class SaveProjectViewModel
    {
        public string Title { get; set; }

        public IEnumerable<SaveProjectClass> ProjectClass { get; set; }
    }
}
