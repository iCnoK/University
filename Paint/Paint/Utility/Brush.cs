using Paint.Utility;
using Paint.Utility.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint.Utility
{
    public class Brush
    {
        public BrushLoader BrushLoader { get; set; }

        private List<KeyValuePair<BrushType, WriteableBitmap>> WriteableBitmaps { get; set; }

        public static WriteableBitmap Resize(WriteableBitmap bitmap, int newXSize, int newYSize)
        {
            return bitmap.Resize(newXSize, newYSize, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
        }

        public WriteableBitmap this[BrushType brush]
        {
            get
            {
                return WriteableBitmaps.Find(x => x.Key == brush).Value;
            }
        }

        public Brush()
        {
            BrushLoader = new BrushLoader();

            WriteableBitmaps = BrushLoader.WriteableBitmaps;
        }
    }
}
