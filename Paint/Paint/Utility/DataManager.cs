using Paint.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Paint.Utility
{
    /// <summary>
    /// Структура для хранения максимального и минимального значений
    /// </summary>
    public class MaxMin
    {
        public int Max { get; private set; }
        public int Min { get; private set; }

        public MaxMin()
        {
            Max = Min = 0;
        }

        public MaxMin(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }
    /// <summary>
    /// Класс для хранения и управления основными данными приложения
    /// </summary>
    public class DataManager
    {
        private Slider BrushSlider { get; set; }

        public Color CurrentColor { get; set; }

        public BrushType BrushType { get; set; }

        public DataManager()
        {
            BrushSlider = new Slider();
            CurrentColor = Colors.White;
        }

        #region Brush Slider Methods
        public MaxMin GetWidthSliderMinMax(BrushType brush)
        {
            return BrushSlider.GetMaxMin(brush);
        }

        public int GetCurrentOpacitySliderValue(BrushType brush)
        {
            return BrushSlider.GetOpacity(brush);
        }

        public int GetCurrentOpacitySliderValueByte(BrushType brush)
        {
            //int percentOpacity = BrushSlider.GetOpacity(brush);
            //double temp = 2.55 * percentOpacity;
            //double need = 255 - temp;
            //int result = 255 - (int)(255 - 2.55 * percentOpacity);
            return 255 - (int)(255 - 2.55 * BrushSlider.GetOpacity(brush));
            //return 255 * (BrushSlider.GetOpacity(brush) / 100);
        }

        public int GetCurrentWidthSliderValue(BrushType brush)
        {
            return BrushSlider.GetWidth(brush);
        }

        public void SetCurrentOpacitySliderValue(BrushType brush, int opacity)
        {
            BrushSlider.SetOpacity(brush, opacity);
        }

        public void SetCurrentWidthSliderValue(BrushType brush, int width)
        {
            BrushSlider.SetWidth(brush, width);
        }
        #endregion

        //#region Color Methods
        //public Color GetColor()
        //{
        //    return CurrentColor;
        //}

        //public void SetColor(Color color)
        //{
        //    CurrentColor = color;
        //}
        //#endregion
    }
}
