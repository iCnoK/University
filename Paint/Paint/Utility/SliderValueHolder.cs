using Paint.Utility.Enums;
using System;
using System.Collections.Generic;

namespace Paint.Utility
{
    class SliderInfo
    {
        public BrushType BrushType { get; private set; }
        public int Minimum { get; private set; }
        public int Maximum { get; private set; }
        /// <summary>
        /// Величина непрозрачности
        /// </summary>
        public int OpacityValue { get; set; }
        /// <summary>
        /// Толщина кисти
        /// </summary>
        public int WidthValue { get; set; }

        public SliderInfo()
        {

        }

        public SliderInfo(int min, int max, BrushType brushType)
        {
            Minimum = min;
            Maximum = max;
            OpacityValue = 100;
            WidthValue = 1;
            BrushType = brushType;
        }

        public override string ToString()
        {
            return BrushType.ToString();
        }
    }

    class Slider
    {
        public SliderInfo this[BrushType type]
        {
            get
            {
                foreach (var item in sliderInfos)
                {
                    if (item.BrushType == type)
                    {
                        return item;
                    }
                }
                throw new Exception();
            }
        }
        public int this[BrushType type, SliderInfoMode mode]
        {
            get
            {
                SliderInfo info = new SliderInfo();
                foreach (var item in sliderInfos)
                {
                    if (item.BrushType == type)
                    {
                        info = item;
                        break;
                    }
                }
                if (mode == SliderInfoMode.OPACITY)
                {
                    return info.OpacityValue;
                }
                return info.WidthValue;
            }
            set
            {
                for (int i = 0; i < sliderInfos.Count; i++)
                {
                    if (sliderInfos[i].BrushType == type)
                    {
                        if (mode == SliderInfoMode.OPACITY)
                        {
                            sliderInfos[i].OpacityValue = value;
                            break;
                        }
                        sliderInfos[i].WidthValue = value;
                        break;
                    }
                }
            }
        }

        private List<SliderInfo> sliderInfos { get; set; }

        public Slider()
        {
            sliderInfos = new List<SliderInfo>();
            sliderInfos.Add(new SliderInfo(1, 100, BrushType.MARKER));
            sliderInfos.Add(new SliderInfo(8, 40, BrushType.FOUNTAINPEN));
            sliderInfos.Add(new SliderInfo(10, 100, BrushType.OILBRUSH));
            sliderInfos.Add(new SliderInfo(5, 200, BrushType.WATERCOLOR));
            sliderInfos.Add(new SliderInfo(1, 100, BrushType.PIXELPEN));
            sliderInfos.Add(new SliderInfo(5, 10, BrushType.PENCIL));
            sliderInfos.Add(new SliderInfo(1, 200, BrushType.ERASER));
            sliderInfos.Add(new SliderInfo(25, 300, BrushType.SPRAYCAN));
            sliderInfos.Add(new SliderInfo(0, 0, BrushType.FILL));
        }
    }
}
