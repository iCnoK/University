using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Paint.Utility
{
    public class ImageChangesHolder
    {
        LinkedList<WriteableBitmap> WriteableBitmaps { get; set; } = null;

        public void Push(WriteableBitmap bitmap)
        {
            WriteableBitmaps.AddLast(bitmap);
            TrimHolder();
        }

        public WriteableBitmap Pop()
        {
            if (WriteableBitmaps.Count != 0)
            {
                var result = WriteableBitmaps.Last.Value;
                WriteableBitmaps.RemoveLast();
                return result;
            }
            return null;
        }

        public ImageChangesHolder()
        {
            WriteableBitmaps = new LinkedList<WriteableBitmap>();
        }

        private void TrimHolder()
        {
            if (WriteableBitmaps.Count > 30)
            {
                WriteableBitmaps.RemoveFirst();
            }
        }
    }
}
