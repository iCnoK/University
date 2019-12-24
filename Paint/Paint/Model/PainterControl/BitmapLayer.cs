using Paint.Utility.Enums;
using System;
using System.Collections.Generic;
using System.IO;
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

        public Color MainColor { get; set; }

        private ImageChangesHolder ChangesHolder { get; set; }

        private bool Changed { get; set; } = false;

        private byte[] Mask { get; set; }

        private WriteableBitmap MaskBitmap { get; set; }

        private Brush Brush { get; set; }

        private int Stride { get; set; }

        private const int StackCapacity = 50;

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

        private BitmapLayer(BitmapLayer bitmapLayer) : this(bitmapLayer.LayerHeight, bitmapLayer.LayerWidth)
        {
            this.Bitmap = bitmapLayer.Bitmap.Clone();
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
            if (!Changed) Changed = true;
            switch (brush)
            {
                case BrushType.MARKER:
                    {
                        DrawWithMarker(coordinates, diameter);
                        return Bitmap;
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
                        DrawWithEraser(coordinates, diameter);
                        break;
                    }
                case BrushType.SPRAYCAN:
                    {
                        break;
                    }
                case BrushType.FILL:
                    {
                        int stride = 0;
                        Bitmap.WritePixels(new Int32Rect(0, 0, Bitmap.PixelWidth, Bitmap.PixelHeight),
                            FillWithColor(coordinates, MainColor, ref stride), stride, 0);
                        break;
                    }
            }
            return null;
        }


        private void DrawWithMarker(Point coordinates, int diameter)
        {
            Int32Rect rect = new Int32Rect(((int)coordinates.X + 300) - diameter / 2, ((int)coordinates.Y + 300) - diameter / 2, diameter, diameter);

            byte[] source = new byte[Stride * diameter];
            Bitmap.CopyPixels(rect, source, Stride, 0);

            byte[] buffer = IntelligentArrayInsertionWithoutCheck(source);

            Bitmap.WritePixels(rect, buffer, Stride, 0);
        }

        private void DrawWithEraser(Point coordinates, int diameter)
        {
            DrawWithMarker(coordinates, diameter);
        }

        private byte[] FillWithColor(Point coordinates, Color color, ref int stride)
        {
            Color controlColor = Bitmap.GetPixel((int)coordinates.X + 300, (int)coordinates.Y + 300);

            int bytesPerPixel = (Bitmap.Format.BitsPerPixel + 7) / 8;
            stride = bytesPerPixel * Bitmap.PixelWidth;

            byte[] array = new byte[stride * Bitmap.PixelHeight];
            Bitmap.CopyPixels(array, stride, 0);
            for (int i = 0; i < array.Length; i += 4)
            {
                if ((array[i] == controlColor.B &&
                    array[i + 1] == controlColor.G &&
                    array[i + 2] == controlColor.R &&
                    array[i + 3] == controlColor.A))
                {
                    array[i] = color.B;
                    array[i + 1] = color.G;
                    array[i + 2] = color.R;
                    array[i + 3] = color.A;
                }
            }
            return array;
        }

        public WriteableBitmap GetWorkspaceImage()
        {
            byte[] carvedImage = new byte[4 * LayerHeight * LayerWidth];
            Int32Rect rect = new Int32Rect(300, 300, LayerWidth, LayerHeight);
            Bitmap.CopyPixels(rect, carvedImage, 4 * LayerWidth, 0);

            WriteableBitmap resultBitmap = new WriteableBitmap(LayerWidth, LayerHeight, 96, 96, PixelFormats.Bgra32, null);
            resultBitmap.WritePixels(new Int32Rect(0, 0, LayerWidth, LayerHeight), carvedImage, 4 * LayerWidth, 0);
            return resultBitmap;
        }

        public byte[] GetWorkspaceArray()
        {
            byte[] carvedImage = new byte[4 * LayerHeight * LayerWidth];
            Int32Rect rect = new Int32Rect(300, 300, LayerWidth, LayerHeight);
            Bitmap.CopyPixels(rect, carvedImage, 4 * LayerWidth, 0);
            return carvedImage;
        }

        public void CopyWorspaceArrayToBitmap(byte[] array)
        {
            Int32Rect rect = new Int32Rect(300, 300, LayerWidth, LayerHeight);
            Bitmap.WritePixels(rect, array, 4 * LayerWidth, 0);
        }

        public static void SaveImageWithFormat(BitmapLayer bitmapLayer, string fileName, ImageFileFormat imageFileFormat)
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
                            bitmapLayer = ChangeBackgroundColor(bitmapLayer, Colors.White);
                            break;
                        }
                    case ImageFileFormat.JPEG:
                        {
                            encoder = new JpegBitmapEncoder();
                            bitmapLayer = ChangeBackgroundColor(bitmapLayer, Colors.White);
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

                encoder.Frames.Add(BitmapFrame.Create(bitmapLayer.GetWorkspaceImage().Clone()));
                encoder.Save(stream);
            }
        }

        private static BitmapLayer ChangeBackgroundColor(BitmapLayer source, Color color)
        {
            Color controlColor = new Color();

            int bytesPerPixel = (source.Bitmap.Format.BitsPerPixel + 7) / 8;
            int stride = bytesPerPixel * source.Bitmap.PixelWidth;

            byte[] array = new byte[stride * source.Bitmap.PixelHeight];
            source.Bitmap.CopyPixels(array, stride, 0);
            for (int i = 0; i < array.Length; i += 4)
            {
                if ((array[i] == controlColor.B &&
                    array[i + 1] == controlColor.G &&
                    array[i + 2] == controlColor.R &&
                    array[i + 3] == controlColor.A))
                {
                    array[i] = color.B;
                    array[i + 1] = color.G;
                    array[i + 2] = color.R;
                    array[i + 3] = color.A;
                }
            }
            source.Bitmap.WritePixels(new Int32Rect(0, 0, source.Bitmap.PixelWidth, source.Bitmap.PixelHeight), array, stride, 0);
            return source;
        }

        private byte[] IntelligentArrayInsertionWithoutCheck(byte[] source)
        {
            Color controlColor = new Color();

            for (int i = 0; i < source.Length; i += 4)
            {
                if ((Mask[i] != controlColor.B ||
                    Mask[i + 1] != controlColor.G ||
                    Mask[i + 2] != controlColor.R ||
                    Mask[i + 3] != controlColor.A))
                {
                    source[i] = Mask[i];
                    source[i + 1] = Mask[i + 1];
                    source[i + 2] = Mask[i + 2];
                    source[i + 3] = Mask[i + 3];
                }
            }
            return source;
        }

        public static byte[] CompareAndConnectArrays(byte[] source, byte[] second)
        {
            Color controlColor = new Color();

            for (int i = 0; i < source.Length; i += 4)
            {
                if ((second[i] != controlColor.B ||
                    second[i + 1] != controlColor.G ||
                    second[i + 2] != controlColor.R ||
                    second[i + 3] != controlColor.A))
                {
                    source[i] = second[i];
                    source[i + 1] = second[i + 1];
                    source[i + 2] = second[i + 2];
                    source[i + 3] = second[i + 3];
                }
            }
            return source;
        }

        private void GetNewMask(BrushType brush, Color color, int diameter, int opacity)
        {
            MainColor = color;
            if (brush == BrushType.ERASER)
            {
                color = Colors.Transparent;
                opacity = color.A;
                brush = BrushType.MARKER;
            }
            if (brush == BrushType.FILL)
            {
                return;
            }
            int bytesPerPixel = (Bitmap.Format.BitsPerPixel + 7) / 8;
            Stride = bytesPerPixel * diameter;
            WriteableBitmap maskBitmap = Brush[brush];
            maskBitmap = Brush.Resize(maskBitmap, diameter, diameter);
            byte[] array = ChangeMaskColor(maskBitmap, color, opacity, diameter);
            maskBitmap.WritePixels(new Int32Rect(0, 0, maskBitmap.PixelWidth, maskBitmap.PixelHeight), array, Stride, 0);
            Mask = array;
            MaskBitmap = maskBitmap;
        }

        private byte[] ChangeMaskColor(WriteableBitmap bitmap, Color newColor, int opacity, int diameter)
        {
            Color controlColor = new Color();
            newColor.A = (byte)opacity;
            byte[] array = new byte[Stride * diameter];
            bitmap.CopyPixels(array, Stride, 0);
            for (int i = 0; i < array.Length; i += 4)
            {
                if ((array[i] != controlColor.B ||
                    array[i + 1] != controlColor.G ||
                    array[i + 2] != controlColor.R ||
                    array[i + 3] != controlColor.A))
                {
                    array[i] = newColor.B;
                    array[i + 1] = newColor.G;
                    array[i + 2] = newColor.R;
                    array[i + 3] = newColor.A;
                }
            }
            return array;
        }

        public static List<BitmapLayer> SyncMask(List<BitmapLayer> bitmapLayers, BrushType brush, Color color, int diameter, int opacity)
        {
            BitmapLayer bitmapLayer = new BitmapLayer(bitmapLayers[0].LayerHeight, bitmapLayers[0].LayerWidth);
            bitmapLayer.UpdateMask(brush, color, diameter, opacity);
            for (int i = 0; i < bitmapLayers.Count; i++)
            {
                bitmapLayers[i].Stride = bitmapLayer.Stride;
                bitmapLayers[i].MaskBitmap = bitmapLayer.MaskBitmap;
                bitmapLayers[i].Mask = bitmapLayer.Mask;
            }
            return bitmapLayers;
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

        public BitmapLayer Clone()
        {
            return new BitmapLayer(this);
        }
    }
}