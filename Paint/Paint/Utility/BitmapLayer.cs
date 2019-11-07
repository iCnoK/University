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
            int bytesPerPixel = (MainLayerBitmap.Format.BitsPerPixel + 7) / 8;
            //int stride = bytesPerPixel * diameter;

            //byte[] colors = new byte[stride * diameter];

            //for (int pixel = 0; pixel < colors.Length; pixel += bytesPerPixel)
            //{
            //    colors[pixel] = color.B;        // blue
            //    colors[pixel + 1] = color.G;    // green
            //    colors[pixel + 2] = color.R;    // red
            //    colors[pixel + 3] = (byte)opacity;    // alpha
            //}

            Int32Rect rect = new Int32Rect((int)coordinates.X - diameter / 2, (int)coordinates.Y - diameter / 2, diameter, diameter);

            MainLayerBitmap.WritePixels(rect, Mask, Stride, 0);
        }

        private void GetNewMask(Color color, int diameter, int opacity)
        {
            int bytesPerPixel = (MainLayerBitmap.Format.BitsPerPixel + 7) / 8;
            Stride = bytesPerPixel * diameter;

            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(diameter, diameter);

            using (System.Drawing.Image i = bitmap)
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(i))
                {
                    using (System.Drawing.Brush b = new System.Drawing.SolidBrush(CustomColorConverter.ConvertFromSWMCToSDC(color)))
                    {
                        g.FillEllipse(b, 0, 0, diameter, diameter);
                    }
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    i.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    Mask = ms.ToArray();
                }
            }

            


            //for (int pixel = 0; pixel < Mask.Length; pixel += bytesPerPixel)
            //{
            //    //Mask[pixel] = color.B;        // blue
            //    //Mask[pixel + 1] = color.G;    // green
            //    //Mask[pixel + 2] = color.R;    // red
            //    //Mask[pixel + 3] = (byte)opacity;    // alpha

            //    if (diameter <= 3)
            //    {
            //        if (diameter == 3)
            //        {
                        
            //            return;
            //        }
            //    }
            //    else
            //    {

            //    }



            //}
        }
    }
}
