//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;

//namespace Paint.Utility.Brushes
//{
//    //TODO: генерация и заполнение массива 1 раз, только при изменении цвета,
//    //      прозрачности и ширины кисти, далее - выполнение присваивания этого 
//    //      массива в IMAGE
//    public static class MarkerBrush
//    {
//        public static WriteableBitmap Draw(WriteableBitmap image, Color color, Point coordinates, int diameter, int opacity)
//        {
//            WriteableBitmap result = image;

//            int bytesPerPixel = (result.Format.BitsPerPixel + 7) / 8;
//            int stride = bytesPerPixel * diameter;

//            byte[] colors = new byte[stride * diameter];

//            //var VARIANT1 = GetByteArray(10000000, bytesPerPixel);

//            //var VARIANT2 = GetByteArray1(10000000, bytesPerPixel);



//            for (int pixel = 0; pixel < colors.Length; pixel += bytesPerPixel)
//            {
//                colors[pixel] = color.B;        // blue
//                colors[pixel + 1] = color.G;    // green
//                colors[pixel + 2] = color.R;    // red
//                colors[pixel + 3] = (byte)opacity;    // alpha
//            }

//            System.Windows.Int32Rect rect = new System.Windows.Int32Rect((int)coordinates.X - diameter / 2, (int)coordinates.Y - diameter / 2, diameter, diameter);

//            result.WritePixels(rect, colors, stride, 0);
//            return result;
//        }

//        //private Point GetCenterPointOfCircle(Point point, int diameter)
//        //{
//        //    Point
//        //}

//        private static byte[] GetColorsArray(int numOfElements, int offset, Color color, int diameter)
//        {
//            byte[] colors = GetTransparentColoredArray(numOfElements, offset);

            



//            return colors;
//        }

//        private static byte[] GetTransparentColoredArray(int numOfElements, int offset)
//        {
//            Color color = Colors.Transparent;
//            byte[] colors = new byte[numOfElements];
//            for (int pixel = 0; pixel < colors.Length; pixel += offset)
//            {
//                colors[pixel] = color.B;        // blue
//                colors[pixel + 1] = color.G;    // green
//                colors[pixel + 2] = color.R;    // red
//                colors[pixel + 3] = color.A;    // alpha
//            }
//            return colors;
//        }

//        //private static byte[] Get

        

//        //private static byte[] GetByteArray1(int numOfElements, int offset)
//        //{
//        //    //Color color = Color.White;
//        //    byte[] colors = new byte[numOfElements];
//        //    TEST(ref colors, 0, offset);
//        //    return colors;
//        //}

//        //private static void TEST(ref byte[] colors, int currentPos, int offset)
//        //{
//        //    Color color = Color.White;
//        //    colors[currentPos] = color.B;        // blue
//        //    colors[currentPos + 1] = color.G;    // green
//        //    colors[currentPos + 2] = color.R;    // red
//        //    colors[currentPos + 3] = color.A;    // alpha
//        //    if (colors.Length > currentPos + offset)
//        //    {
//        //        currentPos += offset;
//        //        TEST(ref colors, currentPos, offset);
//        //    }
//        //    else return;
//        //}
//    }
//}
