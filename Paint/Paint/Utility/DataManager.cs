using Paint.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint.Utility
{
    /// <summary>
    /// Структура для хранения максимального и минимального значения
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
    /// Класс для хранения и управления данными приложения
    /// </summary>
    public class DataManager
    {
        private Slider BrushSlider { get; set; }

        //public ImageChangesHolder ImagesHistory { get; private set; }

        public DataManager()
        {
            BrushSlider = new Slider();
            //ImagesHistory = new ImageChangesHolder();
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
    }
}
