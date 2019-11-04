using Paint.Model.PainterControl;
using Paint.Utility;
using Paint.Utility.Enums;
using Prism.Mvvm;
using System.Windows.Input;
using System.Windows.Media;

namespace Paint.ViewModel
{
    public class PainterViewModel : BindableBase
    {
        private PainterModel PainterModel { get; set; } = new PainterModel();

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

        public string OpenFileDirectory { get; set; } = null;
        //public int CreateImageHeight { get; set; } = 100;
        //public int CreateImageWidth { get; set; } = 100;

        #region Properties
        private int _imageHeight;
        private int _imageWidth;

        private int _xPos;
        private int _yPos;

        private ImageSource _image;
        #endregion

        #region Properties Realization
        public int ImageHeight
        {
            get => _imageHeight;
            set
            {
                _imageHeight = value;
                RaisePropertyChanged("ImageHeight");
            }
        }
        public int ImageWidth
        {
            get => _imageWidth;
            set
            {
                _imageWidth = value;
                RaisePropertyChanged("ImageWidth");
            }
        }
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

        #region Commands
        private ICommand _draw;
        #endregion

        #region Commands Realization
        public ICommand Draw => _draw ?? (_draw =
            new RelayCommand(obj =>
            {
                
            }));
        #endregion

        public PainterViewModel()
        {
            PainterModel.Initialize(500, 500);
            ImageHeight = PainterModel.Image.PixelHeight;
            ImageWidth = PainterModel.Image.PixelWidth;

            Image = PainterModel.Image;

            PainterModel.ImageChanged += PainterModel_ImageChanged;
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
