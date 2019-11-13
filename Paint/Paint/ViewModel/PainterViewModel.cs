using Paint.Model.PainterControl;
using Paint.Utility;
using Paint.Utility.Enums;
using Prism.Mvvm;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Paint.ViewModel
{
    public class PainterViewModel : BindableBase
    {
        private DataManager DataManager { get; set; }

        //private PainterModel PainterModel { get; set; }

        private BitmapLayer BitmapLayer;

        private Timer Timer = new Timer(0.1);

        private PainterModelMode PainterModelMode
        {
            get
            {
                if (OpenFileDirectory == null)
                {
                    return PainterModelMode.NEW;
                }
                return PainterModelMode.SOURCE;
            }
        }

        private Point PreviosPoint { get; set; }

        private Point GetPoint => new Point(XPos, YPos);

        public string OpenFileDirectory { get; set; } = null;

        #region Properties
        private int _imageHeight;
        public int ImageHeight
        {
            get => _imageHeight;
            set
            {
                _imageHeight = value;
                RaisePropertyChanged("ImageHeight");
            }
        }
        private int _imageWidth;
        public int ImageWidth
        {
            get => _imageWidth;
            set
            {
                _imageWidth = value;
                RaisePropertyChanged("ImageWidth");
            }
        }
        private int _xPos;
        public int XPos
        {
            get => _xPos;
            set
            {
                if (value.Equals(_xPos))
                {
                    return;
                }
                _xPos = value;
                RaisePropertyChanged("XPos");
            }
        }
        private int _yPos;
        public int YPos
        {
            get => _yPos;
            set
            {
                if (value.Equals(_yPos))
                {
                    return;
                }
                _yPos = value;
                RaisePropertyChanged("YPos");
            }
        }
        private ImageSource _image;
        public ImageSource Image
        {
            get => _image;
            set
            {
                _image = value;
                RaisePropertyChanged("Image");
            }
        }
        #endregion

        private int _test;
        public int TEST
        {
            get => _test;
            set
            {
                _test = value;
                RaisePropertyChanged("TEST");
            }
        }

        #region Commands
        private ICommand _mouseDown;
        public ICommand MouseDown => _mouseDown ?? (_mouseDown =
            new RelayCommand(obj =>
            {
                Timer.Start();
            }));
        private ICommand _mouseUp;
        public ICommand MouseUp => _mouseUp ?? (_mouseUp =
            new RelayCommand(obj =>
            {
                Timer.Stop();
            }));
        #endregion

        public PainterViewModel() { }

        public PainterViewModel(DataManager dataManager)
        {
            DataManager = dataManager;

            BitmapLayer = new BitmapLayer(500, 500);
            ImageHeight = BitmapLayer.MainLayerBitmap.PixelHeight;
            ImageWidth = BitmapLayer.MainLayerBitmap.PixelWidth;

            Image = BitmapLayer.MainLayerBitmap;

            PreviosPoint = new Point(0, 0);

            BitmapLayer.ImageChanged += PainterModel_ImageChanged;
            Timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);

            DataManager.ParametersChanged += DataManager_ParametersChanged;

            DataManager_ParametersChanged(null, EventArgs.Empty);
            //PainterModel = new PainterModel(dataManager);
            //PainterModel.Initialize(500, 500);
            //ImageHeight = PainterModel.Image.PixelHeight;
            //ImageWidth = PainterModel.Image.PixelWidth;

            //Image = PainterModel.Image;
        }

        private void DataManager_ParametersChanged(object sender, EventArgs e)
        {
            BitmapLayer.UpdateMask(DataManager.BrushType, DataManager.CurrentColor, GetPoint,
                DataManager.GetCurrentWidthSliderValue(DataManager.BrushType),
                DataManager.GetCurrentOpacitySliderValueByte(DataManager.BrushType));
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            switch(DataManager.BrushType)
            {
                //case BrushType.MARKER:
                //    {
                //        if (CheckCoordinationsRepeat()) return;
                //        break;
                //    }
                //case BrushType.FOUNTAINPEN:
                //    {
                //        if (CheckCoordinationsRepeat()) return;
                //        break;
                //    }
                //case BrushType.OILBRUSH:
                //    {
                //        if (CheckCoordinationsRepeat()) return;
                //        break;
                //    }
                case BrushType.WATERCOLOR:
                    {
                        //Timer.Interval = 5;
                        break;
                    }
                //case BrushType.PIXELPEN:
                //    {
                //        if (CheckCoordinationsRepeat()) return;
                //        break;
                //    }
                //case BrushType.PENCIL:
                //    {
                //        if (CheckCoordinationsRepeat()) return;
                //        break;
                //    }
                //case BrushType.ERASER:
                //    {
                //        if (CheckCoordinationsRepeat()) return;
                //        break;
                //    }
                case BrushType.SPRAYCAN:
                    {
                        //Timer.Interval = 5;
                        break;
                    }
                case BrushType.FILL:
                    {
                        if (CheckCoordinationsRepeat()) return;
                        break;
                    }

                default:
                    {
                        //Timer.Interval = 1;
                        if (CheckCoordinationsRepeat()) return;
                        Application.Current.Dispatcher.Invoke(new System.Action(() =>
                            BitmapLayer.Draw(DataManager.BrushType, DataManager.CurrentColor, GetPoint,
                            DataManager.GetCurrentWidthSliderValue(DataManager.BrushType),
                            DataManager.GetCurrentOpacitySliderValueByte(DataManager.BrushType))));
                        break;
                    }
            }



            //if (GetPoint == PreviosPoint)
            //{
            //    return;
            //}
            //PreviosPoint = GetPoint;
            

            TEST++;
        }

        private bool CheckCoordinationsRepeat()
        {
            if (GetPoint == PreviosPoint)
            {
                return true;
            }
            PreviosPoint = GetPoint;
            return false;
        }

        private void PainterModel_ImageChanged(object sender, System.EventArgs e)
        {
            Image = BitmapLayer.MainLayerBitmap;
            ImageHeight = BitmapLayer.MainLayerBitmap.PixelHeight;
            ImageWidth = BitmapLayer.MainLayerBitmap.PixelWidth;

            //Image = PainterModel.Image;
            //ImageHeight = PainterModel.Height;
            //ImageWidth = PainterModel.Width;
        }

        public void Initialize()
        {
            if (PainterModelMode == PainterModelMode.NEW)
            {
                BitmapLayer = new BitmapLayer(ImageHeight, ImageWidth);
                //PainterModel.Initialize(ImageHeight, ImageWidth);
            }
            else if (PainterModelMode == PainterModelMode.SOURCE)
            {
                BitmapLayer = new BitmapLayer(OpenFileDirectory);
                //PainterModel.Initialize(OpenFileDirectory);
            }
        }
    }
}
