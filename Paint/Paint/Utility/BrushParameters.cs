using Paint.Utility.Enums;
using System;
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
    public class BrushParameters
    {
        public event System.EventHandler ParametersChanged;

        protected virtual void OnParametersChanged()
        {
            ParametersChanged?.Invoke(this, EventArgs.Empty);
        }

        private Slider BrushSlider { get; set; }

        private Color _currentColor;
        public Color CurrentColor
        {
            get => _currentColor;
            set
            {
                _currentColor = value;
                OnParametersChanged();
            }
        }

        private BrushType _brushType;
        public BrushType BrushType
        {
            get => _brushType;
            set
            {
                _brushType = value;
                OnParametersChanged();
            }
        }

        public BrushParameters()
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
            return 255 - (int)(255 - 2.55 * BrushSlider.GetOpacity(brush));
        }

        public int GetCurrentWidthSliderValue(BrushType brush)
        {
            return BrushSlider.GetWidth(brush);
        }

        public void SetCurrentOpacitySliderValue(BrushType brush, int opacity)
        {
            BrushSlider.SetOpacity(brush, opacity);
            OnParametersChanged();
        }

        public void SetCurrentWidthSliderValue(BrushType brush, int width)
        {
            BrushSlider.SetWidth(brush, width);
            OnParametersChanged();
        }
        #endregion
    }
}
