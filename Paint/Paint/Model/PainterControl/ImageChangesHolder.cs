using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Paint.Utility
{
    public class ImageChangesHolder
    {
        private const int PercentOfDeletingElements = 20;

        private bool DoublePop { get; set; }

        private Stack<WriteableBitmap> WriteableBitmaps { get; set; } = null;

        private readonly int StackCapacity;

        private int NumberOfDeletingElements
        {
            get
            {
                var temp = PercentOfDeletingElements * 1.0 / 100;
                return (int)(StackCapacity * temp);
            }
        }

        private int StackCount
        {
            get => WriteableBitmaps.Count;
        }

        public void Push(WriteableBitmap bitmap)
        {
            WriteableBitmaps.Push(bitmap.Clone());
            DoublePop = true;
            CheckStackSize();
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

        public void Clear()
        {
            for (int i = 0; i < StackCount; i++)
            {
                Pop();
            }
        }

        public ImageChangesHolder(WriteableBitmap bitmap, int maxCapacity)
        {
            WriteableBitmaps = new Stack<WriteableBitmap>();
            WriteableBitmaps.Push(new WriteableBitmap(bitmap));
            StackCapacity = maxCapacity;
        }

        private void CheckStackSize()
        {
            var length1 = StackCapacity - NumberOfDeletingElements;
            if (StackCount > StackCapacity)
            {
                var newStack = new Stack<WriteableBitmap>();
                var length = StackCapacity - NumberOfDeletingElements;
                for (int i = 0; i <= length; i++)
                {
                    newStack.Push(WriteableBitmaps.Pop().Clone());
                }
                WriteableBitmaps = new Stack<WriteableBitmap>();
                for (int i = 0; i <= length; i++)
                {
                    WriteableBitmaps.Push(newStack.Pop().Clone());
                }
            }
        }
    }
}