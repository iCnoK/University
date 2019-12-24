using Paint.Utility;
using System;
using System.Collections.Generic;

namespace Paint.Model.PainterControl
{
    public class PainterModel
    {
        public event System.EventHandler Initialized;
        protected virtual void OnInitialized()
        {
            Initialized?.Invoke(this, EventArgs.Empty);
        }

        private int _numberOfActivatedLayers;
        public int NumberOfActivatedLayers
        {
            get => _numberOfActivatedLayers;
            set
            {
                _numberOfActivatedLayers = value;
                if (_numberOfActivatedLayers >= 2)
                {
                    IsLayersConnecterOn = true;
                }
                else
                {
                    IsLayersConnecterOn = false;
                }
            }
        }

        public int CurrentLayerIndex { get; set; }

        public List<BitmapLayer> BitmapLayers { get; set; }

        public List<bool> IsCheckedLayers { get; set; }

        public bool IsLayersConnecterOn { get; private set; }

        public BitmapLayer MainBitmap
        {
            get
            {
                if (NumberOfActivatedLayers == 0)
                {
                    return null;
                }

                return BitmapLayers[CurrentLayerIndex];
            }
            set
            {
                BitmapLayers[CurrentLayerIndex] = value;
            }
        }

        public PainterModel()
        {
            IsCheckedLayers = new List<bool>();
            BitmapLayers = new List<BitmapLayer>();
            Initialize(500, 500);
            NumberOfActivatedLayers = 1;
        }

        public void AddLayer()
        {
            BitmapLayers.Add(new BitmapLayer(MainBitmap.LayerHeight, MainBitmap.LayerWidth));
            IsCheckedLayers.Add(false);
        }

        public void SwapLayers(int beginIndex, int destinationIndex)
        {
            BitmapLayers = Move<BitmapLayer>(BitmapLayers, beginIndex, destinationIndex);
            IsCheckedLayers = Move<bool>(IsCheckedLayers, beginIndex, destinationIndex);

        }

        private List<T> Move<T>(List<T> input, int oldIndex, int newIndex)
        {
            List<T> ts = new List<T>(input);

            var currentElement = ts[oldIndex];
            var nextElement = ts[newIndex];

            ts[oldIndex] = nextElement;
            ts[newIndex] = currentElement;
            return ts;
        }

        private void Clear()
        {
            NumberOfActivatedLayers = 1;
            CurrentLayerIndex = 0;
            BitmapLayers.Clear();
            IsCheckedLayers.Clear();
        }

        public void Initialize(int height, int width)
        {
            Clear();
            BitmapLayers.Add(new BitmapLayer(height, width));
            IsCheckedLayers.Add(true);
            OnInitialized();
        }

        public void Initialize(string pathToImage)
        {
            Clear();
            BitmapLayers.Add(new BitmapLayer(pathToImage));
            IsCheckedLayers.Add(true);
            OnInitialized();
        }

        public BitmapLayer CompileAllLayersInOne()
        {
            int index = 0;
            for (int i = 0; i < BitmapLayers.Count; i++)
            {
                if (IsCheckedLayers[i])
                {
                    index = i;
                    break;
                }
            }
            BitmapLayer bitmapLayer = BitmapLayers[index].Clone();
            byte[] bytedBitmapSource = bitmapLayer.GetWorkspaceArray();
            for (int i = 0; i < BitmapLayers.Count; i++)
            {
                byte[] bytedBitmap = null;
                if (IsCheckedLayers[i])
                {
                    bytedBitmap = BitmapLayers[i].GetWorkspaceArray();
                    bytedBitmapSource = BitmapLayer.CompareAndConnectArrays(bytedBitmapSource, 
                        bytedBitmap);
                }
            }
            bitmapLayer.CopyWorspaceArrayToBitmap(bytedBitmapSource);
            return bitmapLayer;
        }
    }
}
