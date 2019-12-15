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

        public List<WriteableBitmap> Markers { get; private set; }
        public List<WriteableBitmap> Erasers { get; private set; }
        public List<WriteableBitmap> PixelPens { get; private set; }

        public static WriteableBitmap Resize(WriteableBitmap bitmap, int newXSize, int newYSize)
        {
            return bitmap.Resize(newXSize, newYSize, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
        }

        public WriteableBitmap this[BrushType brush]
        {
            get
            {
                //if (brush == BrushType.MARKER)
                //{
                //    return Markers[1];
                //}
                //if (brush == BrushType.PIXELPEN)
                //{
                //    return PixelPens[0];
                //}
                //return Markers[0];
                return WriteableBitmaps.Find(x => x.Key == brush).Value;
            }
        }

        public Brush()
        {
            BrushLoader = new BrushLoader();

            WriteableBitmaps = BrushLoader.WriteableBitmaps;

            Markers = BrushLoader.GetBrushesOfSpecialType(BrushType.MARKER);
            Erasers = BrushLoader.GetBrushesOfSpecialType(BrushType.ERASER);
            PixelPens = BrushLoader.GetBrushesOfSpecialType(BrushType.PIXELPEN);
        }
    }
}
