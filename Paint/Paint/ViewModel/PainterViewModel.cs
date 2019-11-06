using Paint.Model.PainterControl;
using Paint.Utility;
using Paint.Utility.Enums;
using Prism.Mvvm;
using System;
using System.Drawing;
using System.Timers;
using System.Windows.Input;
using System.Windows.Media;

namespace Paint.ViewModel
{
    public class PainterViewModel : BindableBase
    {
        private DataManager DataManager { get; set; }

        private PainterModel PainterModel { get; set; } = new PainterModel();

        private Timer Timer = new Timer(5);

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

        //public SliderInfo SliderInfo { get; set; }

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

            PainterModel.Initialize(500, 500);
            ImageHeight = PainterModel.Image.PixelHeight;
            ImageWidth = PainterModel.Image.PixelWidth;

            Image = PainterModel.Image;
            PreviosPoint = new Point(0, 0);

            PainterModel.ImageChanged += PainterModel_ImageChanged;
            Timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (GetPoint == PreviosPoint)
            {
                return;
            }
            PreviosPoint = GetPoint;
            TEST++;
        }

        private void PainterModel_ImageChanged(object sender, System.EventArgs e)
        {
            Image = PainterModel.Image;
            ImageHeight = PainterModel.Height;
            ImageWidth = PainterModel.Width;
        }

        public void Initialize()
        {
            if (PainterModelMode == PainterModelMode.NEW)
            {
                PainterModel.Initialize(ImageHeight, ImageWidth);
            }
            else if (PainterModelMode == PainterModelMode.SOURCE)
            {
                PainterModel.Initialize(OpenFileDirectory);
            }
        }


    }
}
