using Paint.Model.PainterControl;
using Paint.Utility;
using Paint.Utility.Enums;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Paint.ViewModel
{
    public class PainterViewModel : BindableBase
    {
        private BrushParameters BrushParameters { get; set; }

        public LayerBarViewModel LayerBarStatus { get; set; }

        public PainterModel PainterModel { get; set; }

        //private PainterModel PainterModel { get; set; }

        //private List<>

        //private BitmapLayer BitmapLayer;

        private Timer Timer = new Timer(1);

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

        public string SaveFileDirectory { get; set; } = null;

        #region Properties
        private int DiameterOfEllipse { get; set; }

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
                XEllipsePos = value - EllipseDiameter / 2;
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
                YEllipsePos = value - EllipseDiameter / 2;
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
        private int _ellipseDiameter;
        public int EllipseDiameter
        {
            get => _ellipseDiameter;
            set
            {
                _ellipseDiameter = value;
                RaisePropertyChanged("EllipseDiameter");
            }
        }
        private int _xEllipsePos;
        public int XEllipsePos
        {
            get => _xEllipsePos;
            set
            {
                _xEllipsePos = value;
                RaisePropertyChanged("XEllipsePos");
            }
        }
        private int _yEllipsePos;
        public int YEllipsePos
        {
            get => _yEllipsePos;
            set
            {
                _yEllipsePos = value;
                RaisePropertyChanged("YEllipsePos");
            }
        }
        #endregion

        //private int _test;
        //public int TEST
        //{
        //    get => _test;
        //    set
        //    {
        //        _test = value;
        //        RaisePropertyChanged("TEST");
        //    }
        //}

        #region Commands
        private ICommand _mouseDown;
        public ICommand MouseDown => _mouseDown ?? (_mouseDown =
            new DelegateCommand(delegate ()
            {
                Timer.Start();
            }));
        private ICommand _mouseUp;
        public ICommand MouseUp => _mouseUp ?? (_mouseUp =
            new DelegateCommand(delegate ()
            {
                Timer.Stop();
                //IsFirstClick = true;
                SaveChanges();
            }));
        #endregion

        public PainterViewModel() { }

        public PainterViewModel(BrushParameters brushParameters, LayerBarViewModel viewModel)
        {
            LayerBarStatus = viewModel;

            PainterModel = new PainterModel();

            BrushParameters = brushParameters;

            PainterModel.MainBitmap = new BitmapLayer(500, 500);
            ImageHeight = PainterModel.MainBitmap.LayerHeight;
            ImageWidth = PainterModel.MainBitmap.LayerWidth;

            Image = PainterModel.MainBitmap.Bitmap;

            PreviosPoint = new Point(0, 0);

            PainterModel.MainBitmap.ImageChanged += BitmapLayer_ImageChanged; 

            Timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);

            BrushParameters.ParametersChanged += DataManager_ParametersChanged;

            DataManager_ParametersChanged(null, EventArgs.Empty);
            //PainterModel = new PainterModel(dataManager);
            //PainterModel.Initialize(500, 500);
            //ImageHeight = PainterModel.Image.PixelHeight;
            //ImageWidth = PainterModel.Image.PixelWidth;

            //Image = PainterModel.Image;
            LayerBarStatus.Items.Add(new Item(PainterModel.MainBitmap.Bitmap, true));
        }

        private void BitmapLayer_ImageChanged(object sender, EventArgs e)
        {
            Image = PainterModel.MainBitmap.Bitmap;
            ImageHeight = PainterModel.MainBitmap.LayerHeight;
            ImageWidth = PainterModel.MainBitmap.LayerWidth;
        }

        private void DataManager_ParametersChanged(object sender, EventArgs e)
        {
            //BitmapLayer.UpdateMask(DataManager.BrushType, DataManager.CurrentColor, GetPoint,
            //    DataManager.GetCurrentWidthSliderValue(DataManager.BrushType));
            PainterModel.MainBitmap.UpdateMask(BrushParameters.BrushType, BrushParameters.CurrentColor,
                BrushParameters.GetCurrentWidthSliderValue(BrushParameters.BrushType),
                BrushParameters.GetCurrentOpacitySliderValueByte(BrushParameters.BrushType));
            EllipseDiameter = BrushParameters.GetCurrentWidthSliderValue(BrushParameters.BrushType);
        }
        
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Color ahbwfbawf = Colors.Transparent;

            int diameter = BrushParameters.GetCurrentWidthSliderValue(BrushParameters.BrushType);

            //if (CheckForMouseAcceleration(diameter))
            //{
            //    //TEST = 123456789;
            //}

            if (CheckCoordinationsRepeat())
            {
                return;
            }

            Application.Current.Dispatcher.Invoke(new System.Action(() =>
                PainterModel.MainBitmap.Draw(BrushParameters.BrushType, GetPoint, diameter)));


            //switch(BrushParameters.BrushType)
            //{

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
            //case BrushType.WATERCOLOR:
            //    {
            //        //Timer.Interval = 5;
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
            //case BrushType.SPRAYCAN:
            //    {
            //        break;
            //    }
            //case BrushType.FILL:
            //    {
            //        if (CheckCoordinationsRepeat()) return;
            //        break;
            //    }
            //case BrushType.PIXELPEN:
            //    {
            //        if (CheckCoordinationsRepeat()) return;
            //        break;
            //    }

            //default:
            //    {
            //        //Timer.Interval = 1;
            //        if (CheckCoordinationsRepeat()) return;
            //        Application.Current.Dispatcher.Invoke(new System.Action(() =>
            //            BitmapLayer.Draw(BrushParameters.BrushType, GetPoint,
            //            BrushParameters.GetCurrentWidthSliderValue(BrushParameters.BrushType))));
            //        break;
            //    }
            //}



            //if (GetPoint == PreviosPoint)
            //{
            //    return;
            //}
            //PreviosPoint = GetPoint;


            //TEST++;
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

        //private bool IsFirstClick = true;
        //private bool CheckForMouseAcceleration(int diameter)
        //{
        //    if (IsFirstClick)
        //    {
        //        return IsFirstClick = false;
        //    }
        //    Point currentPoint = GetPoint;
        //    //Point previousPoint = PreviosPoint;

        //    //var test1 = Math.Abs(currentPoint.X - PreviosPoint.X);
        //    //var test2 = Math.Abs(currentPoint.Y - PreviosPoint.Y);

        //    if (Math.Abs(currentPoint.X - PreviosPoint.X) > diameter
        //        || Math.Abs(currentPoint.Y - PreviosPoint.Y) > diameter)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        public void SavePicture(string fileName)
        {
            if (fileName != null)
            {
                string completedFileName = $"{Path.GetDirectoryName(fileName)}\\{Path.GetFileNameWithoutExtension(fileName)}";
                SaveFileDirectory = fileName;
                ImageFileFormat fileFormat = BitmapLayer.GetImageFileFormat(fileName);
                if (fileFormat != ImageFileFormat.UNKNOWN)
                {
                    PainterModel.MainBitmap.SaveImageWithFormat(completedFileName, fileFormat);
                }
                else
                {
                    throw new Exception("Неизвестный формат изображения");
                }
            }
        }

        public void SaveChanges()
        {
            PainterModel.MainBitmap.HoldCurrentImage();
        }

        public void UndoChanges()
        {
            PainterModel.MainBitmap.ReturnPreviousState();
        }

        public void Initialize()
        {
            if (PainterModelMode == PainterModelMode.NEW)
            {
                PainterModel.MainBitmap.Initialize(ImageHeight, ImageWidth);
            }
            else if (PainterModelMode == PainterModelMode.SOURCE)
            {
                PainterModel.MainBitmap.Initialize(OpenFileDirectory);
            }
        }
    }
}