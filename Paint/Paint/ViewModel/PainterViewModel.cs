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
using System.Windows.Media.Imaging;

namespace Paint.ViewModel
{
    public class PainterViewModel : BindableBase
    {
        private BrushParameters BrushParameters { get; set; }

        public LayerBarViewModel LayerBarStatus { get; set; }

        public PainterModel PainterModel { get; set; }

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
                if (PainterModel.MainBitmap != null)
                {
                    SaveChanges();
                }
            }));
        #endregion

        public PainterViewModel() { }

        public PainterViewModel(BrushParameters brushParameters, LayerBarViewModel viewModel)
        {
            LayerBarStatus = viewModel;

            PainterModel = new PainterModel();

            BrushParameters = brushParameters;

            //PainterModel.MainBitmap = new BitmapLayer(500, 500);
            ImageHeight = 500;
            ImageWidth = 500;

            Initialize();
            PainterModel_Initialized(null, EventArgs.Empty);

            Image = PainterModel.MainBitmap.Bitmap;

            PreviosPoint = new Point(0, 0);

            PainterModel.MainBitmap.ImageChanged += BitmapLayer_ImageChanged;

            Timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);

            BrushParameters.ParametersChanged += DataManager_ParametersChanged;

            DataManager_ParametersChanged(null, EventArgs.Empty);

            PainterModel.Initialized += PainterModel_Initialized;

            LayerBarStatus.AddItem += LayerBarStatus_AddItem;
            LayerBarStatus.DeleteItem += LayerBarStatus_DeleteItem;
            LayerBarStatus.ItemChanged += LayerBarStatus_ItemChanged;
            LayerBarStatus.ItemIndexChanged += LayerBarStatus_ItemIndexChanged;
            //PainterModel = new PainterModel(dataManager);
            //PainterModel.Initialize(500, 500);
            //ImageHeight = PainterModel.Image.PixelHeight;
            //ImageWidth = PainterModel.Image.PixelWidth;

            //Image = PainterModel.Image;

            //LayerBarStatus.Items.Add(new Item(PainterModel.MainBitmap.Bitmap, 1, true));
            //LayerBarStatus.Items.Add(new Item(PainterModel.MainBitmap.Bitmap, 2, false));
            //LayerBarStatus.Items.Add(new Item(PainterModel.MainBitmap.Bitmap, 3, true));
            //LayerBarStatus.Items.Add(new Item(PainterModel.MainBitmap.Bitmap, 4, true));

        }

        private void PainterModel_Initialized(object sender, EventArgs e)
        {
            LayerBarStatus.Clear();
            LayerBarStatus.AddItemIntoCollection(PainterModel.BitmapLayers[0].GetWorkspaceImage(), true);
        }

        private void LayerBarStatus_AddItem(object sender, EventArgs e)
        {
            PainterModel.AddLayer();
            PainterModel.BitmapLayers[PainterModel.BitmapLayers.Count - 1].ImageChanged += BitmapLayer_ImageChanged;
            LayerBarStatus.AddItemIntoCollection(PainterModel.BitmapLayers[PainterModel.BitmapLayers.Count - 1].GetWorkspaceImage().Clone());
            DataManager_ParametersChanged(null, EventArgs.Empty);
            //WriteableBitmap writeableBitmap = PainterModel.BitmapLayers[0].GetWorkspaceImage();
            //LayerBarStatus.AddItemIntoCollection(writeableBitmap.Clone());
        }

        private void LayerBarStatus_DeleteItem(object sender, EventArgs e)
        {
            if (PainterModel.BitmapLayers.Count > 1)
            {
                PainterModel.BitmapLayers[PainterModel.CurrentLayerIndex].ImageChanged -= BitmapLayer_ImageChanged;
                PainterModel.BitmapLayers.RemoveAt(PainterModel.CurrentLayerIndex);
                PainterModel.IsCheckedLayers.RemoveAt(PainterModel.CurrentLayerIndex);
                LayerBarStatus.DeleteItemFromCollection(PainterModel.CurrentLayerIndex);
            }
        }

        private void LayerBarStatus_ItemChanged(object sender, EventArgs e)
        {
            PainterModel.NumberOfActivatedLayers = LayerBarStatus.NumberOfActivatedLayers;
            for (int i = 0; i < LayerBarStatus.Items.Count; i++)
            {
                PainterModel.IsCheckedLayers[i] = LayerBarStatus.Items[i].IsCheckedElement;
            }
            DataManager_ParametersChanged(null, EventArgs.Empty);
            if (PainterModel.IsLayersConnecterOn)
            {
                Application.Current.Dispatcher.Invoke(new System.Action(() =>
                Image = PainterModel.CompileAllLayersInOne().Bitmap));
            }
        }

        private void LayerBarStatus_ItemIndexChanged(object sender, EventArgs e)
        {
            if (LayerBarStatus.ItemIndex >= 0)
            {
                PainterModel.CurrentLayerIndex = LayerBarStatus.ItemIndex;
            }
            else
            {
                PainterModel.CurrentLayerIndex = 0;
            }
            //if (PainterModel.NumberOfActivatedLayers == 1)
            //{
            //    Image = PainterModel.MainBitmap.Bitmap;
            //}
            //if (PainterModel.NumberOfActivatedLayers > 1 && PainterModel.IsCheckedLayers[PainterModel.CurrentLayerIndex] == false)
            //{
                
            //}
            if (PainterModel.MainBitmap.Bitmap != null)
            {
                Image = PainterModel.MainBitmap.Bitmap;
            }
        }

        private void BitmapLayer_ImageChanged(object sender, EventArgs e)
        {
            Image = PainterModel.MainBitmap.Bitmap;
            ImageHeight = PainterModel.MainBitmap.LayerHeight;
            ImageWidth = PainterModel.MainBitmap.LayerWidth;

            Application.Current.Dispatcher.Invoke(new System.Action(() =>
                LayerBarStatus.Items[PainterModel.CurrentLayerIndex].ImageElementSource =
                PainterModel.BitmapLayers[PainterModel.CurrentLayerIndex].GetWorkspaceImage()), System.Windows.Threading.DispatcherPriority.Background);
        }

        private void DataManager_ParametersChanged(object sender, EventArgs e)
        {
            PainterModel.BitmapLayers = BitmapLayer.SyncMask(PainterModel.BitmapLayers,
                BrushParameters.BrushType, BrushParameters.CurrentColor,
                BrushParameters.GetCurrentWidthSliderValue(BrushParameters.BrushType),
                BrushParameters.GetCurrentOpacitySliderValueByte(BrushParameters.BrushType));

            EllipseDiameter = BrushParameters.GetCurrentWidthSliderValue(BrushParameters.BrushType);
        }

        //System.Threading.Thread Thread = new System.Threading.Thread(
        //    new System.Threading.ParameterizedThreadStart(TestFunk(Image, PainterModel)));

        //public static void TestFunk(ImageSource image, PainterModel painterModel)
        //{
        //    image = painterModel.CompileAllLayersInOne().Bitmap;
        //}

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            int diameter = BrushParameters.GetCurrentWidthSliderValue(BrushParameters.BrushType);

            if (CheckCoordinationsRepeat())
            {
                return;
            }

            if (PainterModel.MainBitmap != null)
            {
                Application.Current.Dispatcher.Invoke(new System.Action(() =>
                PainterModel.MainBitmap.Draw(BrushParameters.BrushType, GetPoint, diameter)), System.Windows.Threading.DispatcherPriority.Render);

                if (PainterModel.IsLayersConnecterOn)
                {
                    Application.Current.Dispatcher.Invoke(new System.Action(() =>
                    Image = PainterModel.CompileAllLayersInOne().Bitmap), System.Windows.Threading.DispatcherPriority.Render);
                }
                Application.Current.Dispatcher.Invoke(new System.Action(() =>
                LayerBarStatus.Items[PainterModel.CurrentLayerIndex].ImageElementSource = 
                PainterModel.BitmapLayers[PainterModel.CurrentLayerIndex].GetWorkspaceImage()), System.Windows.Threading.DispatcherPriority.Background);


                //for (int i = 0; i < LayerBarStatus.Items.Count; i++)
                //{
                //    LayerBarStatus.Items[i].ImageElementSource = PainterModel.BitmapLayers[i].GetWorkspaceImage();
                //}
            }
        }

        //private void TestFunk()
        //{
        //    Image = PainterModel.CompileAllLayersInOne().Bitmap
        //}

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
                    Application.Current.Dispatcher.Invoke(new System.Action(() =>
                    BitmapLayer.SaveImageWithFormat(PainterModel.CompileAllLayersInOne(), completedFileName, fileFormat)));
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
                PainterModel.Initialize(ImageHeight, ImageWidth);
                //PainterModel.MainBitmap.Initialize(ImageHeight, ImageWidth);
                //PainterModel.Clear();
            }
            else if (PainterModelMode == PainterModelMode.SOURCE)
            {
                PainterModel.Initialize(OpenFileDirectory);
                //PainterModel.MainBitmap.Initialize(OpenFileDirectory);
                //PainterModel.Clear();
            }
        }
    }
}