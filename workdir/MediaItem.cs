using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace workdir
{
    class MediaItem
    {
        public BitmapImage Thumbnail { get; set; }
        public int Duration { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
    }
}
