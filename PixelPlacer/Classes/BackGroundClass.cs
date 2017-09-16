using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// class used in VideoTypesViewModel
namespace PixelPlacer.Classes
{
    public class BackGroundClass
    {
        public string VTitle { get; set; }
        public int VId { get; set; }
        public string VFilePath { get; set; }
        public int VTypeId { get; set; }
        public int ProjId {get; set;}
        public int ProjVidId { get; set; }
        public bool BackGround { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
    }
}
