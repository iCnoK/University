using Paint.Utility.Enums;
using System;
using System.Collections.Generic;

namespace Paint.Utility
{
    public class SliderInfo
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

    public class Slider
    {
        //public SliderInfo this[BrushType type]
        //{
        //    get
        //    {
        //        foreach (var item in sliderInfos)
        //        {
        //            if (item.BrushType == type)
        //            {
        //                return item;
        //            }
        //        }
        //        throw new Exception();
        //    }
        //}

        private int this[BrushType brush]
        {
            get
            {
                switch (brush)
                {
                    case BrushType.MARKER: return 0;
                    case BrushType.FOUNTAINPEN: return 1;
                    case BrushType.OILBRUSH: return 2;
                    case BrushType.WATERCOLOR: return 3;
                    case BrushType.PIXELPEN: return 4;
                    case BrushType.PENCIL: return 5;
                    case BrushType.ERASER: return 6;
                    case BrushType.SPRAYCAN: return 7;
                    case BrushType.FILL: return 8;
                }
                return 0;
            }
        }

        public MaxMin GetMaxMin(BrushType brush)
        {
            var temp = sliderInfos[this[brush]];
            return new MaxMin(temp.Minimum, temp.Maximum);
        }

        public int GetOpacity(BrushType brush)
        {
            return sliderInfos[this[brush]].OpacityValue;
        }

        public void SetOpacity(BrushType brush, int opacity)
        {
            if (opacity < 0 || opacity > 100)
            {
                throw new ArgumentOutOfRangeException();
            }
            sliderInfos[this[brush]].OpacityValue = opacity;
        }

        public int GetWidth(BrushType brush)
        {
            return sliderInfos[this[brush]].WidthValue;
        }

        public void SetWidth(BrushType brush, int width)
        {
            if (width < 0 || width > 300)
            {
                throw new ArgumentOutOfRangeException();
            }
            sliderInfos[this[brush]].WidthValue = width;
        }

        //public int this[BrushType type, SliderMode mode]
        //{
        //    get
        //    {
        //        SliderInfo info = new SliderInfo();
        //        foreach (var item in sliderInfos)
        //        {
        //            if (item.BrushType == type)
        //            {
        //                info = item;
        //                break;
        //            }
        //        }
        //        if (mode == SliderMode.OPACITY)
        //        {
        //            return info.OpacityValue;
        //        }
        //        return info.WidthValue;
        //    }
        //    set
        //    {
        //        for (int i = 0; i < sliderInfos.Count; i++)
        //        {
        //            if (sliderInfos[i].BrushType == type)
        //            {
        //                if (mode == SliderMode.OPACITY)
        //                {
        //                    sliderInfos[i].OpacityValue = value;
        //                    break;
        //                }
        //                sliderInfos[i].WidthValue = value;
        //                break;
        //            }
        //        }
        //    }
        //}

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
