using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageShare.Data
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public DateTime Uploaded { get; set; }
        public int Likes { get; set; }
        
    }
}
