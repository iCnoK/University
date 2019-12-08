using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Paint.Utility
{
    public class ImageChangesHolder
    {
        private bool DoublePop { get; set; }

        private Stack<WriteableBitmap> WriteableBitmaps { get; set; } = null;

        public void Push(WriteableBitmap bitmap)
        {
            //if (WriteableBitmaps.Count != 0)
            //{
            //var tempBitmap = WriteableBitmaps.Peek();
            //if (bitmap != tempBitmap)
            //{
            WriteableBitmaps.Push(bitmap.Clone());
            //}
            DoublePop = true;
            //}
        }

        public WriteableBitmap Pop()
        {
            if (WriteableBitmaps.Count != 1)
            {
                if (DoublePop)
                {
                    DoublePop = false;
                    WriteableBitmaps.Pop();
                    var result = WriteableBitmaps.Pop();
                    Push(result);
                    return new WriteableBitmap(result);
                }
                return new WriteableBitmap(WriteableBitmaps.Pop());
            }
            else
            {
                return new WriteableBitmap(WriteableBitmaps.Peek());
            }
        }

        public ImageChangesHolder(WriteableBitmap bitmap)
        {
            WriteableBitmaps = new Stack<WriteableBitmap>();
            WriteableBitmaps.Push(new WriteableBitmap(bitmap));
        }
    }
}
