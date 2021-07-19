using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageShare.Data;

namespace ImageShare.Web.Models
{
    public class ImageViewModel
    {
        public Image Image { get; set; }
        public bool CanLike { get; set; }
    }
}
