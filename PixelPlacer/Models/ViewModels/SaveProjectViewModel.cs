using PixelPlacer.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ViewModel used in ProjectsController for SaveProject Action AJAX call
// Updates title in Project Table and returns an IEnumerable of SaveProjectClass
// which updates the ProjectVides Table with coordinates
namespace PixelPlacer.Models.ViewModels
{
    public class SaveProjectViewModel
    {
        public string Title { get; set; }

        public IEnumerable<SaveProjectClass> ProjectClass { get; set; }
    }
}
