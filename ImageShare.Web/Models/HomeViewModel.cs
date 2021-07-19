using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageShare.Data;

namespace ImageShare.Web.Models
{
    public class HomeViewModel
    {
        public List<Image> Images { get; set; }
    }
}
