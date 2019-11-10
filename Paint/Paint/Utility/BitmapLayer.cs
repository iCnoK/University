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

        private WriteableBitmap MaskBitmap { get; set; }

        private Brush Brush { get; set; }

        private int Stride { get; set; }

        public bool IsMainLayer { get; private set; }

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
                Brush = new Brush();
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
                Brush = new Brush();
                OnImageChanged();
            }
            else
            {
                throw new Exception("Высота и/или ширина не может быть меньше 1");
            }
        }

        public void UpdateMask(BrushType brush, Color color, Point coordinates, int diameter, int opacity)
        {
            GetNewMask(brush, color, diameter, opacity);
        }

        public WriteableBitmap Draw(BrushType brush, Color color, Point coordinates, int diameter, int opacity)
        {
            switch (brush)
            {
                case BrushType.MARKER:
                    {
                        DrawWithMarker(color, coordinates, diameter);
                        return MainLayerBitmap;
                    }
                case BrushType.FOUNTAINPEN:
                    {
                        DrawWithMarker(color, coordinates, diameter);
                        return MainLayerBitmap;
                    }
                case BrushType.OILBRUSH:
                    {
                        DrawWithMarker(color, coordinates, diameter);
                        return MainLayerBitmap;
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

        private void DrawWithMarker(Color color, Point coordinates, int diameter)
        {
            Int32Rect rect = new Int32Rect((int)coordinates.X - diameter / 2, (int)coordinates.Y - diameter / 2, diameter, diameter);

            byte[] source = new byte[Stride * diameter];
            MainLayerBitmap.CopyPixels(rect, source, Stride, 0);
            WriteableBitmap bitmap = new WriteableBitmap(diameter, diameter, 96, 96, PixelFormats.Bgra32, null);
            bitmap.WritePixels(new Int32Rect(0, 0, diameter, diameter), source, Stride, 0);

            Mask = IntelligentArrayInsertion(bitmap);

            MainLayerBitmap.WritePixels(rect, Mask, Stride, 0);
        }

        private byte[] IntelligentArrayInsertion(WriteableBitmap bitmap)
        {
            byte[] result = new byte[Stride * bitmap.PixelHeight];

            Color controlColor = new Color();

            for (int x = 0; x < bitmap.PixelHeight; x++)
            {
                for (int y = 0; y < bitmap.PixelWidth; y++)
                {
                    Color tested = MaskBitmap.GetPixel(x, y);
                    if (tested != controlColor)
                    {
                        bitmap.SetPixel(x, y, tested);
                    }
                }
            }

            bitmap.CopyPixels(result, Stride, 0);
            return result;
        }



        private void GetNewMask(BrushType brush, Color color, int diameter, int opacity)
        {
            int bytesPerPixel = (MainLayerBitmap.Format.BitsPerPixel + 7) / 8;
            Stride = bytesPerPixel * diameter;

            //Brush brush1 = new Brush();

            //BitmapImage bitmapImage = new BitmapImage(new Uri(@"P:\GitHub Repositories\iCnoK\University\Paint\Paint\TempRecources\ERASER.png"));
            //bitmapImage.CreateOptions = BitmapCreateOptions.None;
            //var temp = new WriteableBitmap(bitmapImage);
            //BitmapImage bitmapImage1 = new BitmapImage(new Uri(@"P:\GitHub Repositories\iCnoK\University\Paint\Paint\TempRecources\FOUNTAINPEN.png"));
            //bitmapImage.CreateOptions = BitmapCreateOptions.None;
            //var temp1 = new WriteableBitmap(bitmapImage1);
            //BitmapImage bitmapImage2 = new BitmapImage(new Uri(@"P:\GitHub Repositories\iCnoK\University\Paint\Paint\TempRecources\MARKER.png"));
            //bitmapImage.CreateOptions = BitmapCreateOptions.None;
            //var temp2 = new WriteableBitmap(bitmapImage2);
            //BitmapImage bitmapImage3 = new BitmapImage(new Uri(@"P:\GitHub Repositories\iCnoK\University\Paint\Paint\TempRecources\OILBRUSH.png"));
            //bitmapImage.CreateOptions = BitmapCreateOptions.None;
            //var temp3 = new WriteableBitmap(bitmapImage3);
            //BitmapImage bitmapImage4 = new BitmapImage(new Uri(@"P:\GitHub Repositories\iCnoK\University\Paint\Paint\TempRecources\PENCIL.png"));
            //bitmapImage.CreateOptions = BitmapCreateOptions.None;
            //var temp4 = new WriteableBitmap(bitmapImage4);
            //BitmapImage bitmapImage5 = new BitmapImage(new Uri(@"P:\GitHub Repositories\iCnoK\University\Paint\Paint\TempRecources\PIXELPEN.png"));
            //bitmapImage.CreateOptions = BitmapCreateOptions.None;
            //var temp5 = new WriteableBitmap(bitmapImage5);
            //BitmapImage bitmapImage6 = new BitmapImage(new Uri(@"P:\GitHub Repositories\iCnoK\University\Paint\Paint\TempRecources\SPRAYCAN.png"));
            //bitmapImage.CreateOptions = BitmapCreateOptions.None;
            //var temp6 = new WriteableBitmap(bitmapImage6);
            //BitmapImage bitmapImage7 = new BitmapImage(new Uri(@"P:\GitHub Repositories\iCnoK\University\Paint\Paint\TempRecources\WATERCOLOR.png"));
            //bitmapImage.CreateOptions = BitmapCreateOptions.None;
            //var temp7 = new WriteableBitmap(bitmapImage7);


            //brush1.BrushLoader.AddBrush(temp, BrushType.ERASER);
            //brush1.BrushLoader.AddBrush(temp1, BrushType.FOUNTAINPEN);
            //brush1.BrushLoader.AddBrush(temp2, BrushType.MARKER);
            //brush1.BrushLoader.AddBrush(temp3, BrushType.OILBRUSH);
            //brush1.BrushLoader.AddBrush(temp4, BrushType.PENCIL);
            //brush1.BrushLoader.AddBrush(temp5, BrushType.PIXELPEN);
            //brush1.BrushLoader.AddBrush(temp6, BrushType.SPRAYCAN);
            //brush1.BrushLoader.AddBrush(temp7, BrushType.WATERCOLOR);
            //brush1.BrushLoader.SaveBrushes();





            WriteableBitmap maskBitmap = Brush[brush];

            ChangeMaskColor(ref maskBitmap, color, opacity);

            maskBitmap = Brush.Resize(maskBitmap, diameter, diameter);

            //Mask = maskBitmap.ToByteArray();

            ////Brush brush = new Brush();

            //WriteableBitmap writeableBitmap = brush[BrushType.MARKER];
            //var temp = Brush.Resize(writeableBitmap, diameter, diameter);

            byte[] array = new byte[Stride * diameter];

            maskBitmap.CopyPixels(array, Stride, 0);

            Mask = array;

            MaskBitmap = maskBitmap;

        }

        private void ChangeMaskColor(ref WriteableBitmap bitmap, Color newColor, int opacity)
        {
            Color defaultColor = new Color();
            newColor.A = (byte)opacity;

            for (int x = 0; x < bitmap.PixelHeight; x++)
            {
                for (int y = 0; y < bitmap.PixelWidth; y++)
                {
                    Color testColor = bitmap.GetPixel(x, y);
                    if (testColor != defaultColor)
                    {
                        bitmap.SetPixel(x, y, newColor);
                    }
                }
            }
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