using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AS_TestProject.Models
{
    public class PhotoJudgment
    {
        public PhotoJudgment()
        {
            this.PhotoList = new List<GalleryPhoto>();
        }

        public List<GalleryPhoto> PhotoList { get; set; }
    }
}