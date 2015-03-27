using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace editor
{
    class Graphic
    {
        public static Bitmap bmpImage;
        public static Image CropImage(Image img, Rectangle cropArea)
        {
            if(bmpImage==null)
                bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        public static Image GetImage(Image img,string vals)
        {
            if (vals == null)
                return null;
            vals = vals.Replace("\0", "");
            vals = Path.GetFileName(vals);
            var id = Helper.FindCoord(vals);

            return CropImage(Helper._img, new Rectangle(id[0], id[1], 32, 32));
        }
    }
}
