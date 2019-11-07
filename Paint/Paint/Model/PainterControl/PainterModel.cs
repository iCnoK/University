using Paint.Utility;
using Paint.Utility.Brushes;
using Paint.Utility.Enums;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint.Model.PainterControl
{
    public class PainterModel
    {
        public event System.EventHandler ImageChanged;

        protected virtual void OnImageChanged()
        {
            ImageChanged?.Invoke(this, EventArgs.Empty);
        }

        private DataManager DataManager { get; set; }

        public WriteableBitmap Image { get; private set; }

        public int Height => Image.PixelHeight;

        public int Width => Image.PixelWidth;

        private ImageChangesHolder ImageChangesHolder { get; set; } = new ImageChangesHolder();
        
        public PainterModel(DataManager dataManager)
        {
            DataManager = dataManager;
            //Initialize(500, 500);
        }

        /// <summary>
        /// Сохранить состояние картинки
        /// </summary>
        public void HoldCurrentImage()
        {
            ImageChangesHolder.Push(Image);
        }

        /// <summary>
        /// Вернуть предыдущее состояние картинки
        /// </summary>
        public void ReturnPreviousState()
        {
            var state = ImageChangesHolder.Pop();
            if (state != null)
            {
                Image = state;
            }
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
                Image = new WriteableBitmap(bitmapImage);
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
                Image = new WriteableBitmap(width, height, 96, 96, System.Windows.Media.PixelFormats.Bgra32, null);

                //Fill(System.Drawing.Color.White);

                OnImageChanged();
            }
            else
            {
                throw new Exception("Высота и/или ширина не может быть меньше 1");
            }
        }

        /// <summary>
        /// Заполнить картинку выбранным цветом
        /// </summary>
        /// <param name="color"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void Fill(Color color)
        {
            int bytesPerPixel = (Image.Format.BitsPerPixel + 7) / 8;
            int stride = bytesPerPixel * Width;

            byte[] colors = new byte[stride * Height];
            for (int pixel = 0; pixel < colors.Length; pixel += bytesPerPixel)
            {
                colors[pixel] = color.B;        // blue
                colors[pixel + 1] = color.G;    // green
                colors[pixel + 2] = color.R;    // red
                colors[pixel + 3] = color.A;    // alpha
            }

            Int32Rect rect = new Int32Rect(0, 0, Width, Height);

            Image.WritePixels(rect, colors, stride, 0);
        }

        public void DrawWithSelectedBrush(BrushType brush, Color color, Point coordinates)
        {
            switch (brush)
            {
                case BrushType.MARKER:
                    {
                        Image = MarkerBrush.Draw(Image, color, coordinates, DataManager.GetCurrentWidthSliderValue(brush),
                            DataManager.GetCurrentOpacitySliderValueByte(brush));
                        break;
                    }
                //case BrushType.FOUNTAINPEN:
                //    {
                //        break;
                //    }
                //case BrushType.OILBRUSH:
                //    {
                //        break;
                //    }
                //case BrushType.WATERCOLOR:
                //    {
                //        break;
                //    }
                //case BrushType.PIXELPEN:
                //    {
                //        break;
                //    }
                //case BrushType.PENCIL:
                //    {
                //        break;
                //    }
                //case BrushType.ERASER:
                //    {
                //        break;
                //    }
                //case BrushType.SPRAYCAN:
                //    {
                //        break;
                //    }
                //case BrushType.FILL:
                //    {
                //        break;
                //    }
            }
        }



        /// <summary>
        /// Смешать указанные цвета
        /// </summary>
        /// <param name="color"></param>
        /// <param name="backColor"></param>
        /// <param name="amount"></param>
        //public static Color Blend(Color color, Color backColor, double amount)
        //{
        //    byte r = (byte)((color.R * amount) + backColor.R * (1 - amount));
        //    byte g = (byte)((color.G * amount) + backColor.G * (1 - amount));
        //    byte b = (byte)((color.B * amount) + backColor.B * (1 - amount));
        //    return Color.FromArgb(r, g, b);
        //}

        //public static ImageSource BitmapToImageSource(Bitmap bitmap)
        //{
        //    int xCount = bitmap.Width;
        //    int yCount = bitmap.Height;

        //    var stride = (xCount * 3 + (xCount % 4));
        //    byte[] pixels = new byte[yCount * stride];

        //    System.Drawing.Color color;

        //    for (var x = 0; x < xCount; x++)
        //    {
        //        for (var y = 0; y < yCount; y++)
        //        {
        //            color = bitmap.GetPixel(x, y);

        //            pixels[y * stride + x * 3 + 0] = color.R;
        //            pixels[y * stride + x * 3 + 1] = color.G;
        //            pixels[y * stride + x * 3 + 2] = color.B;
        //        }
        //    }

        //    var result = BitmapSource.Create(xCount, yCount, 96, 96, PixelFormats.Rgb24, BitmapPalettes.WebPalette, pixels, stride);

        //    return result;
        //}
    }
}
