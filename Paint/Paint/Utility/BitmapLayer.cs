using Paint.Utility.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint.Utility
{
    struct MaskParameters
    {
        Color Color;

        int Diameter;

        int Opacity;

        BrushType BrushType;

        public MaskParameters(Color color, int diameter, int opacity, BrushType brushType)
        {
            Color = color;
            Diameter = diameter;
            Opacity = opacity;
            BrushType = brushType;
        }

        public static bool IsChanged(MaskParameters previos, MaskParameters next)
        {
            if (previos.Color != next.Color)
            {
                return true;
            }

            if (previos.BrushType != next.BrushType)
            {
                return true;
            }

            if (previos.Diameter != next.Diameter)
            {
                return true;
            }

            if (previos.Opacity != next.Opacity)
            {
                return true;
            }

            return false;
        }
    }

    public class BitmapLayer
    {
        public event System.EventHandler ImageChanged;

        protected virtual void OnImageChanged()
        {
            ImageChanged?.Invoke(this, EventArgs.Empty);
        }

        public WriteableBitmap MainLayerBitmap { get; private set; }

        private ImageChangesHolder ChangesHolder { get; set; }

        private bool Changed { get; set; }

        private byte[] Mask { get; set; }

        private byte[][][] ThreeDemMask { get; set; }

        private int Stride { get; set; }

        private MaskParameters? PreviosParameters { get; set; } = null;

        public bool IsMainLayer { get; private set; }

        //public int PixelHeight => MainLayerBitmap.PixelHeight;

        //public int PixelWidth => MainLayerBitmap.PixelWidth;

        //public BrushType BrushType { get; private set; }

        public BitmapLayer() : this(500, 500)
        {

        }

        public BitmapLayer(string pathToFile)
        {
            if (pathToFile != null)
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(pathToFile));
                bitmapImage.CreateOptions = BitmapCreateOptions.None;
                MainLayerBitmap = new WriteableBitmap(bitmapImage);
                OnImageChanged();
            }
            else
            {
                throw new Exception("Путь к файлу равняется null");
            }
        }

        public BitmapLayer(int height, int width)
        {
            if (height > 1 && width > 1)
            {
                MainLayerBitmap = new WriteableBitmap(width, height, 96, 96, System.Windows.Media.PixelFormats.Bgra32, null);

                OnImageChanged();
            }
            else
            {
                throw new Exception("Высота и/или ширина не может быть меньше 1");
            }
        }

        public WriteableBitmap Draw(BrushType brush, Color color, Point coordinates, int diameter, int opacity)
        {
            if (PreviosParameters != null && MaskParameters.IsChanged(
                (MaskParameters)PreviosParameters, new MaskParameters(color, diameter, opacity, brush)))
            {
                //SomeFunctionToCreateMask
                GetNewMask(color, diameter, opacity);
            }
            else
            {
                GetNewMask(color, diameter, opacity);
                PreviosParameters = new MaskParameters(color, diameter, opacity, brush);
            }
            PreviosParameters = new MaskParameters(color, diameter, opacity, brush);

            switch (brush)
            {
                case BrushType.MARKER:
                    {
                        DrawWithMarker(color, coordinates, diameter, opacity);
                        return MainLayerBitmap;
                    }
                case BrushType.FOUNTAINPEN:
                    {
                        break;
                    }
                case BrushType.OILBRUSH:
                    {
                        break;
                    }
                case BrushType.WATERCOLOR:
                    {
                        break;
                    }
                case BrushType.PIXELPEN:
                    {
                        break;
                    }
                case BrushType.PENCIL:
                    {
                        break;
                    }
                case BrushType.ERASER:
                    {
                        break;
                    }
                case BrushType.SPRAYCAN:
                    {
                        break;
                    }
                case BrushType.FILL:
                    {
                        break;
                    }
            }
            return null;
        }

        private void DrawWithMarker(Color color, Point coordinates, int diameter, int opacity)
        {
            Int32Rect rect = new Int32Rect((int)coordinates.X - diameter / 2, (int)coordinates.Y - diameter / 2, diameter, diameter);

            MainLayerBitmap.WritePixels(rect, Mask, Stride, 0);
        }

        private void GetNewMask(Color color, int diameter, int opacity)
        {
            int bytesPerPixel = (MainLayerBitmap.Format.BitsPerPixel + 7) / 8;
            Stride = bytesPerPixel * diameter;

            //Directory.CreateDirectory("Brushes");

            Brush brush = new Brush();

            BitmapImage bitmapImage = new BitmapImage(new Uri(@"C:\Users\Андрей\Desktop\icons8-круг-100.png"));
            bitmapImage.CreateOptions = BitmapCreateOptions.None;
            WriteableBitmap bitmap = new WriteableBitmap(bitmapImage);

            //brush.BrushLoader.AddBrush(bitmap, BrushType.MARKER);
            //brush.BrushLoader.SaveBrushes();
            
            WriteableBitmap writeableBitmap = brush[BrushType.MARKER];
            var temp = Brush.Resize(writeableBitmap, diameter, diameter);

            byte[] test = new byte[4 * diameter * diameter];

            temp.CopyPixels(test, 4 * diameter, 0);

            Mask = test;

        }

        private void FillWithColor(ref byte[] array, Color color)
        {
            int bytesPerPixel = (MainLayerBitmap.Format.BitsPerPixel + 7) / 8;

            for (int pixel = 0; pixel < array.Length; pixel += bytesPerPixel)
            {
                array[pixel] = color.B;        // blue
                array[pixel + 1] = color.G;    // green
                array[pixel + 2] = color.R;    // red
                array[pixel + 3] = color.A;    // alpha
            }
        }

        //private void Fill3DemMaskWithColor(Color color)
        //{
        //    for (int i = 0; i < ThreeDemMask.Length; i++)
        //    {
        //        for (int j = 0; j < ThreeDemMask[i].Length; j++)
        //        {
        //            for (int k = 0; k < ThreeDemMask[i][j].Length; k++)
        //            {

        //            }
        //        }
        //    }
        //}

        private byte[][][] GetThreeDimensionalArrayAndFill(int firstDem, int secondDem, int thirdDem, Color color)
        {
            byte[][][] result = new byte[firstDem][][];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new byte[secondDem][];
            }
            for (int i = 0; i < result.Length; i++)
            {
                for (int j = 0; j < result[i].Length; j++)
                {
                    result[i][j] = new byte[thirdDem];
                    result[i][j][0] = color.B;
                    result[i][j][1] = color.G;
                    result[i][j][2] = color.R;
                    result[i][j][3] = color.A;
                }
            }
            return result;
        }

        //private byte[,] FlipArray(byte[,] array)
        //{
        //    int firstLength = array.GetLength(0);
        //    int secondLength = array.GetLength(1);
        //    byte[,] result = new byte[firstLength, secondLength];

        //    for (int I = 0, i = firstLength - 1; I < firstLength; I++, i--)
        //    {
        //        for (int J = 0, j = secondLength - 1; J < secondLength; J++, j--)
        //        {
        //            result[I, J] = array[i, j];
        //        }
        //    }

        //    return result;
        //}

        //private byte[] StickArrays(byte[,] leftPeace, byte[,] rightPeace)
        //{
        //    int arrayheight = leftPeace.GetLength(0);
        //    int arrayWidth = leftPeace.GetLength(1) + rightPeace.GetLength(1);

        //    int leftWidth = leftPeace.GetLength(1);
        //    int rightWidth = rightPeace.GetLength(1);

        //    byte[] result = new byte[arrayheight * arrayWidth];

        //    for (int i = 0, firstDem = 0, secondDem = 0, iterator = 0; i < result.Length;
        //        iterator++) 
        //    {
        //        while (true)
        //        {
        //            result[i] = leftPeace[iterator, firstDem];
        //            if (firstDem + 1 >= leftWidth)
        //            {
        //                i++;
        //                break;
        //            }
        //            i++;
        //            firstDem++;
        //        }
        //        while (true)
        //        {
        //            result[i] = rightPeace[iterator, secondDem];
        //            if (secondDem +1 >= rightWidth)
        //            {
        //                i++;
        //                break;
        //            }
        //            i++;
        //            secondDem++;
        //        }
        //        firstDem = secondDem = 0;
        //    }
        //    return result;
        //}
    }
}



//System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(diameter, diameter);

//using (System.Drawing.Image i = bitmap)
//{
//    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(i))
//    {
//        using (System.Drawing.Brush b = new System.Drawing.SolidBrush(CustomColorConverter.ConvertFromSWMCToSDC(color)))
//        {
//            g.FillEllipse(b, 0, 0, diameter, diameter);
//        }
//    }

//    using (MemoryStream ms = new MemoryStream())
//    {
//        i.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
//        Mask = ms.ToArray();
//    }
//}