﻿using Paint.Utility.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint.Utility
{
    //public class VisibilityToIntConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if ((Visibility)value == Visibility.Visible)
    //        {
    //            return 5;
    //        }
    //        return 0;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if ((int)value == 5)
    //        {
    //            return Visibility.Visible;
    //        }
    //        return Visibility.Collapsed;
    //    }
    //}

    //public class VisibilityToBool : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if ((Visibility)value == Visibility.Visible)
    //        {
    //            return false;
    //        }
    //        return true;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if ((bool)value == false)
    //        {
    //            return Visibility.Visible;
    //        }
    //        return Visibility.Collapsed;
    //    }
    //}

    //public class BoolNegation : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if ((bool)value)
    //        {
    //            return false;
    //        }
    //        return true;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if ((bool)value)
    //        {
    //            return false;
    //        }
    //        return true;
    //    }
    //}

    
    [ValueConversion(typeof(int), typeof(bool))]
    public class WidthToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //int test = System.Convert.ToInt32(value);
            //if (test >= 100 && test <= 300)
            if (System.Convert.ToInt32(value) == 300)
            {
                return false;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == false)
            {
                return 300;
            }
            return 0;
        }
    }

    [ValueConversion(typeof(int), typeof(bool))]
    public class WidthToBoolReverse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //int test = System.Convert.ToInt32(value);
            //if (test >= 200 && test <= 300)
            if (System.Convert.ToInt32(value) == 300)
            {
                return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true)
            {
                return 300;
            }
            return 0;
        }
    }

    [ValueConversion(typeof(int), typeof(int))]
    public class WidthToInt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //int test = System.Convert.ToInt32(value);
            //if (test >= 100 && test <= 300)
            if (System.Convert.ToInt32(value) == 300)
            {
                return 5;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 5)
            {
                return 300;
            }
            return 0;
        }
    }

    [ValueConversion(typeof(BrushType), typeof(string))]
    public class BrushTypeToString : IValueConverter
    {
        const string _marker = "Маркер";
        const string _fountainPen = "Перьевая ручка";
        const string _oilBrush = "Кисть для масляных красок";
        const string _watercolor = "Акварель";
        const string _pixelPen = "Пиксельное перо";
        const string _pencil = "Карандаш";
        const string _eraser = "Ластик";
        const string _sprayCan = "Баллончик с краской";
        const string _fill = "Заполнить";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((BrushType)value)
            {
                case BrushType.MARKER: return _marker;
                case BrushType.FOUNTAINPEN: return _fountainPen;
                case BrushType.OILBRUSH: return _oilBrush;
                case BrushType.WATERCOLOR: return _watercolor;
                case BrushType.PIXELPEN: return _pixelPen;
                case BrushType.PENCIL: return _pencil;
                case BrushType.ERASER: return _eraser;
                case BrushType.SPRAYCAN: return _sprayCan;
                case BrushType.FILL: return _fill;
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.Equals((string)value, _marker)) return BrushType.MARKER;
            if (string.Equals((string)value, _fountainPen)) return BrushType.FOUNTAINPEN;
            if (string.Equals((string)value, _oilBrush)) return BrushType.OILBRUSH;
            if (string.Equals((string)value, _watercolor)) return BrushType.WATERCOLOR;
            if (string.Equals((string)value, _pixelPen)) return BrushType.PIXELPEN;
            if (string.Equals((string)value, _pencil)) return BrushType.PENCIL;
            if (string.Equals((string)value, _eraser)) return BrushType.ERASER;
            if (string.Equals((string)value, _sprayCan)) return BrushType.SPRAYCAN;
            if (string.Equals((string)value, _fill)) return BrushType.FILL;
            throw new NotImplementedException();
        }
    }

    //public class BitmapToImageSource : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        using (MemoryStream memory = new MemoryStream())
    //        {
    //            Bitmap bitmap = (Bitmap)value;
    //            bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
    //            memory.Position = 0;
    //            BitmapImage bitmapimage = new BitmapImage();
    //            bitmapimage.BeginInit();
    //            bitmapimage.StreamSource = memory;
    //            bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
    //            bitmapimage.EndInit();

    //            return bitmapimage;
    //        }
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        return value;
    //    }
    //}
}
