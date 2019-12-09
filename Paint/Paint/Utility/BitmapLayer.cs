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
    public class BitmapLayer
    {
        public event System.EventHandler ImageChanged;

        protected virtual void OnImageChanged()
        {
            ImageChanged?.Invoke(this, EventArgs.Empty);
        }

        public WriteableBitmap Bitmap { get; private set; }

        public int LayerHeight => Bitmap.PixelHeight - 600;

        public int LayerWidth => Bitmap.PixelWidth - 600;

        private ImageChangesHolder ChangesHolder { get; set; }

        //private bool Changed { get; set; }

        private byte[] Mask { get; set; }

        private WriteableBitmap MaskBitmap { get; set; }

        private Brush Brush { get; set; }

        private int Stride { get; set; }

        private const int StackCapacity = 50;

        //public bool IsMainLayer { get; private set; }

        public BitmapLayer() : this(500, 500)
        {

        }

        public BitmapLayer(string pathToFile)
        {
            Initialize(pathToFile);
            Brush = new Brush();
        }

        public BitmapLayer(int height, int width)
        {
            Initialize(height, width);
            Brush = new Brush();
        }

        /// <summary>
        /// Сохранить состояние картинки
        /// </summary>
        public void HoldCurrentImage()
        {
            ChangesHolder.Push(Bitmap.Clone());
        }

        /// <summary>
        /// Вернуть предыдущее состояние картинки
        /// </summary>
        public void ReturnPreviousState()
        {
            var state = ChangesHolder.Pop();
            if (state != null)
            {
                Bitmap = state;
                //OnImageChanged();
            }
            OnImageChanged();
        }

        /// <summary>
        /// Инициализация для загрузки существующего изображения
        /// </summary>
        public void Initialize(string pathToFile)
        {
            if (pathToFile != null)
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(pathToFile));
                bitmapImage.CreateOptions = BitmapCreateOptions.None;
                var tempBitmap = new WriteableBitmap(bitmapImage);

                WriteableBitmap resultBitmap = new WriteableBitmap(tempBitmap.PixelWidth + 600, tempBitmap.PixelHeight + 600,
                    96, 96, PixelFormats.Bgra32, null);

                byte[] temp = new byte[4 * tempBitmap.PixelWidth * tempBitmap.PixelHeight];
                tempBitmap.CopyPixels(new Int32Rect(0, 0, tempBitmap.PixelWidth, tempBitmap.PixelHeight), temp,
                    4 * tempBitmap.PixelWidth, 0);

                resultBitmap.WritePixels(new Int32Rect(300, 300, tempBitmap.PixelWidth,
                    tempBitmap.PixelHeight), temp, 4 * tempBitmap.PixelWidth, 0);

                Bitmap = resultBitmap;

                ChangesHolder = new ImageChangesHolder(Bitmap, StackCapacity);
                OnImageChanged();
            }
            else
            {
                throw new Exception("Путь к файлу равняется null");
            }
        }

        /// <summary>
        /// Инициализация для создания пустой картинки с выбранными высотой и шириной
        /// </summary>
        public void Initialize(int height, int width)
        {
            if (height > 1 && width > 1)
            {
                Bitmap = new WriteableBitmap(width + 600, height + 600, 96, 96, System.Windows.Media.PixelFormats.Bgra32, null);
                ChangesHolder = new ImageChangesHolder(Bitmap, StackCapacity);
                OnImageChanged();
            }
            else
            {
                throw new Exception("Высота и/или ширина не может быть меньше 1");
            }
        }

        /// <summary>
        /// Обновить маску кисти с новыми параметрами
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="color"></param>
        /// <param name="diameter"></param>
        /// <param name="opacity"></param>
        public void UpdateMask(BrushType brush, Color color, int diameter, int opacity)
        {
            GetNewMask(brush, color, diameter, opacity);
        }

        /// <summary>
        /// Нарисовать маску кисти на холсте
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="color"></param>
        /// <param name="coordinates"></param>
        /// <param name="diameter"></param>
        /// <returns></returns>
        public WriteableBitmap Draw(BrushType brush, Point coordinates, int diameter)
        {
            switch (brush)
            {
                case BrushType.MARKER:
                    {
                        DrawWithMarker(coordinates, diameter);
                        return Bitmap;
                        //break;
                    }
                case BrushType.FOUNTAINPEN:
                    {
                        //DrawWithMarker(coordinates, diameter);
                        //return Bitmap;
                        break;
                    }
                case BrushType.OILBRUSH:
                    {
                        //DrawWithMarker(coordinates, diameter);
                        //return Bitmap;
                        break;
                    }
                case BrushType.WATERCOLOR:
                    {
                        break;
                    }
                case BrushType.PIXELPEN:
                    {
                        //DrawWithMarker2_0(coordinates, diameter);
                        break;
                    }
                case BrushType.PENCIL:
                    {
                        break;
                    }
                case BrushType.ERASER:
                    {
                        DrawWithEraser(coordinates, diameter);
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

        //public WriteableBitmap DrawWithAcceleration(BrushType brush, Point previous, Point next, int diameter)
        //{
        //    switch (brush)
        //    {
        //        case BrushType.MARKER:
        //            {
        //                DrawWithMarker(previous, next, diameter);
        //                //DrawWithMarker(coordinates, diameter);
        //                return Bitmap;
        //                //break;
        //            }
        //        case BrushType.FOUNTAINPEN:
        //            {
        //                break;
        //            }
        //        case BrushType.OILBRUSH:
        //            {
        //                break;
        //            }
        //        case BrushType.WATERCOLOR:
        //            {
        //                break;
        //            }
        //        case BrushType.PIXELPEN:
        //            {
        //                //DrawWithMarker2_0(coordinates, diameter);
        //                break;
        //            }
        //        case BrushType.PENCIL:
        //            {
        //                break;
        //            }
        //        case BrushType.ERASER:
        //            {
        //                //DrawWithMarker(coordinates, diameter);
        //                break;
        //            }
        //        case BrushType.SPRAYCAN:
        //            {
        //                break;
        //            }
        //        case BrushType.FILL:
        //            {
        //                break;
        //            }
        //    }
        //    return null;
        //}
    

        private void DrawWithMarker(Point coordinates, int diameter)
        {
            Int32Rect rect = new Int32Rect(((int)coordinates.X + 300) - diameter / 2, ((int)coordinates.Y + 300) - diameter / 2, diameter, diameter);

            byte[] source = new byte[Stride * diameter];
            Bitmap.CopyPixels(rect, source, Stride, 0);

            byte[] buffer = IntelligentArrayInsertion(source);

            Bitmap.WritePixels(rect, buffer, Stride, 0);
        }

        private void DrawWithEraser(Point coordinates, int diameter)
        {
            DrawWithMarker(coordinates, diameter);
        }

        //private void DrawWithMarker(Point previous, Point next, int diameter)
        //{
        //    Bitmap.DrawLine((int)previous.X, (int)previous.Y, (int)next.X, (int)next.Y);
        //    //Int32Rect rect = new Int32Rect(((int)coordinates.X + 300) - diameter / 2, ((int)coordinates.Y + 300) - diameter / 2, diameter, diameter);

        //    //byte[] source = new byte[Stride * diameter];
        //    //Bitmap.CopyPixels(rect, source, Stride, 0);

        //    //byte[] buffer = IntelligentArrayInsertion(source);

        //    //Bitmap.WritePixels(rect, buffer, Stride, 0);
        //}

        private WriteableBitmap GetWorkspaceImage()
        {
            byte[] carvedImage = new byte[4 * LayerHeight * LayerWidth];
            Int32Rect rect = new Int32Rect(300, 300, LayerWidth, LayerHeight);
            Bitmap.CopyPixels(rect, carvedImage, 4 * LayerWidth, 0);

            WriteableBitmap resultBitmap = new WriteableBitmap(LayerWidth, LayerHeight, 96, 96, PixelFormats.Bgra32, null);
            resultBitmap.WritePixels(new Int32Rect(0, 0, LayerWidth, LayerHeight), carvedImage, 4 * LayerWidth, 0);
            return resultBitmap;
        }

        //private WriteableBitmap DeleteTransparentFromBitmap(WriteableBitmap bitmap)
        //{
        //    WriteableBitmap bitmapCopy = bitmap.Clone();
        //    for (int x = 0; x < bitmapCopy.PixelHeight; x++)
        //    {
        //        for (int y = 0; y < bitmapCopy.PixelWidth; y++)
        //        {
        //            Color testColor = bitmapCopy.GetPixel(x, y);
        //            if (testColor == Colors.Transparent)
        //            {
        //                bitmapCopy.SetPixel(x, y, new Color());
        //            }
        //        }
        //    }
        //    return bitmapCopy;
        //}

        /// <summary>
        /// Сохранить изображение с выбранным названием и форматом
        /// </summary>
        /// <param name="fileName">Только название, без формата</param>
        /// <param name="imageFileFormat"></param>
        public void SaveImageWithFormat(string fileName, ImageFileFormat imageFileFormat)
        {
            string fileExtension = imageFileFormat.ToString().ToLower();
            fileName += $".{fileExtension}";
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                BitmapEncoder encoder = null;
                switch (imageFileFormat)
                {
                    case ImageFileFormat.BMP:
                        {
                            encoder = new BmpBitmapEncoder();
                            break;
                        }
                    case ImageFileFormat.JPEG:
                        {
                            encoder = new JpegBitmapEncoder();
                            break;
                        }
                    case ImageFileFormat.PNG:
                        {
                            encoder = new PngBitmapEncoder();
                            break;
                        }
                    case ImageFileFormat.TIFF:
                        {
                            encoder = new TiffBitmapEncoder();
                            break;
                        }
                }
                WriteableBitmap cuttedBitmap = GetWorkspaceImage().Clone();

                encoder.Frames.Add(BitmapFrame.Create(GetWorkspaceImage().Clone()));
                encoder.Save(stream);
            }
        }

        private byte[] IntelligentArrayInsertion(byte[] source)
        {
            Color controlColor = new Color();

            for (int i = 0; i < source.Length; i += 4) 
            {
                if (Mask[i]     != controlColor.B ||
                    Mask[i + 1] != controlColor.G ||
                    Mask[i + 2] != controlColor.R ||
                    Mask[i + 3] != controlColor.A)
                {
                    source[i] = Mask[i];
                    source[i + 1] = Mask[i + 1];
                    source[i + 2] = Mask[i + 2];
                    source[i + 3] = Mask[i + 3];
                }
            }
            return source;
        }

        private void GetNewMask(BrushType brush, Color color, int diameter, int opacity)
        {
            if (brush == BrushType.ERASER)
            {
                color = Colors.Transparent;
                opacity = 0;
            }

            if (brush == BrushType.FILL)
            {
                return;
            }

            int bytesPerPixel = (Bitmap.Format.BitsPerPixel + 7) / 8;
            Stride = bytesPerPixel * diameter;

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

        public static ImageFileFormat GetImageFileFormat(string fileName)
        {
            string extension = Path.GetExtension(fileName);

            if (string.Equals(extension, ".tiff")) return ImageFileFormat.TIFF;
            if (string.Equals(extension, ".png")) return ImageFileFormat.PNG;
            if (string.Equals(extension, ".bmp")) return ImageFileFormat.BMP;
            if (string.Equals(extension, ".jpg")) return ImageFileFormat.JPEG;
            if (string.Equals(extension, ".jpeg")) return ImageFileFormat.JPEG;
            return ImageFileFormat.UNKNOWN;
        }

        //private void FillWithColor(ref byte[] array, Color color)
        //{
        //    int bytesPerPixel = (MainLayerBitmap.Format.BitsPerPixel + 7) / 8;

        //    for (int pixel = 0; pixel < array.Length; pixel += bytesPerPixel)
        //    {
        //        array[pixel] = color.B;        // blue
        //        array[pixel + 1] = color.G;    // green
        //        array[pixel + 2] = color.R;    // red
        //        array[pixel + 3] = color.A;    // alpha
        //    }
        //}

        /// <summary>
        /// мусор
        /// </summary>
        //private void CreateAndSerializeBrushes()
        //{
        //    Brush brush1 = new Brush();

        //    BitmapImage bitmapImage = new BitmapImage(new Uri(@"Z:\GitHub Repositories\University\Paint\Paint\TempRecources\ERASER.png"));
        //    bitmapImage.CreateOptions = BitmapCreateOptions.None;
        //    var temp = new WriteableBitmap(bitmapImage);
        //    BitmapImage bitmapImage1 = new BitmapImage(new Uri(@"Z:\GitHub Repositories\University\Paint\Paint\TempRecources\FOUNTAINPEN.png"));
        //    bitmapImage.CreateOptions = BitmapCreateOptions.None;
        //    var temp1 = new WriteableBitmap(bitmapImage1);
        //    BitmapImage bitmapImage2 = new BitmapImage(new Uri(@"Z:\GitHub Repositories\University\Paint\Paint\TempRecources\Marker1.png"));
        //    bitmapImage.CreateOptions = BitmapCreateOptions.None;
        //    var temp2 = new WriteableBitmap(bitmapImage2);
        //    BitmapImage bitmapImage3 = new BitmapImage(new Uri(@"Z:\GitHub Repositories\University\Paint\Paint\TempRecources\OILBRUSH.png"));
        //    bitmapImage.CreateOptions = BitmapCreateOptions.None;
        //    var temp3 = new WriteableBitmap(bitmapImage3);
        //    BitmapImage bitmapImage4 = new BitmapImage(new Uri(@"Z:\GitHub Repositories\University\Paint\Paint\TempRecources\PENCIL.png"));
        //    bitmapImage.CreateOptions = BitmapCreateOptions.None;
        //    var temp4 = new WriteableBitmap(bitmapImage4);
        //    BitmapImage bitmapImage5 = new BitmapImage(new Uri(@"Z:\GitHub Repositories\University\Paint\Paint\TempRecources\PIXELPEN.png"));
        //    bitmapImage.CreateOptions = BitmapCreateOptions.None;
        //    var temp5 = new WriteableBitmap(bitmapImage5);
        //    BitmapImage bitmapImage6 = new BitmapImage(new Uri(@"Z:\GitHub Repositories\University\Paint\Paint\TempRecources\SPRAYCAN.png"));
        //    bitmapImage.CreateOptions = BitmapCreateOptions.None;
        //    var temp6 = new WriteableBitmap(bitmapImage6);
        //    BitmapImage bitmapImage7 = new BitmapImage(new Uri(@"Z:\GitHub Repositories\University\Paint\Paint\TempRecources\WATERCOLOR.png"));
        //    bitmapImage.CreateOptions = BitmapCreateOptions.None;
        //    var temp7 = new WriteableBitmap(bitmapImage7);


        //    brush1.BrushLoader.AddBrush(temp, BrushType.ERASER);
        //    brush1.BrushLoader.AddBrush(temp1, BrushType.FOUNTAINPEN);
        //    brush1.BrushLoader.AddBrush(temp2, BrushType.MARKER);
        //    brush1.BrushLoader.AddBrush(temp3, BrushType.OILBRUSH);
        //    brush1.BrushLoader.AddBrush(temp4, BrushType.PENCIL);
        //    brush1.BrushLoader.AddBrush(temp5, BrushType.PIXELPEN);
        //    brush1.BrushLoader.AddBrush(temp6, BrushType.SPRAYCAN);
        //    brush1.BrushLoader.AddBrush(temp7, BrushType.WATERCOLOR);
        //    brush1.BrushLoader.SaveBrushes();
        //}
    }
}