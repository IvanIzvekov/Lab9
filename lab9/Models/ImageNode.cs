using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab9.Models
{
    public class ImageNode
    {
        public string ImageSource { get; }
        public ImageNode(string source)
        {
            ImageSource = source;
        }
    }
}