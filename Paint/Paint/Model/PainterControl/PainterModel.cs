using Paint.Utility;
using System.Collections.Generic;

namespace Paint.Model.PainterControl
{
    public class PainterModel
    {
        public int NumberOfActivatedLayers { get; set; }

        public int CurrentLayerIndex { get; set; }

        public List<BitmapLayer> BitmapLayers { get; set; }

        public BitmapLayer MainBitmap { get; set; }

        public void AddLayer()
        {
            BitmapLayers.Add(new BitmapLayer(MainBitmap.LayerHeight, MainBitmap.LayerWidth));
        }

        private void CompileAllLayersInOne()
        {
            for (int i = 0; i < BitmapLayers.Count; i++)
            {

            }
        }

        public PainterModel()
        {
            //BitmapLayers.Add(MainBitmap);
        }
    }
}
