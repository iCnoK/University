using Paint.Utility;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Input;

namespace Paint.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        private BrushParameters BrushParameters = new BrushParameters();

        public SideMenuViewModel SideMenuStatus { get; set; }
        public BrushesBarViewModel BrushesBarStatus { get; set; }
        public PainterViewModel PainterStatus { get; set; }

        private ICommand _openMenu;
        public ICommand OpenMenu => _openMenu ?? (_openMenu = new DelegateCommand(delegate ()
        {
            SideMenuStatus.ChangeVisibilityOfMenu = Visibility.Visible;
        }));

        private ICommand _undoChanges;
        public ICommand UndoChanges => _undoChanges ?? (_undoChanges = new DelegateCommand(delegate ()
        {
            PainterStatus.UndoChanges();
        }));

        public MainWindowViewModel()
        {
            SideMenuStatus = new SideMenuViewModel();
            BrushesBarStatus = new BrushesBarViewModel(BrushParameters);
            PainterStatus = new PainterViewModel(BrushParameters);

            SideMenuStatus.OpenFileChanged += OpenFileChangedEventHandler;
            SideMenuStatus.SaveFileChanged += SaveFileChangedEventHandler;
            SideMenuStatus.ImageSizeChanged += ImageSizeChangedEventHandler;

            //BrushesBarStatus.BrushChanged += BrushChangedEventHandler;
            //BrushesBarStatus.ColorChanged += ColorChangedEventHandler;
        }

        private void ImageSizeChangedEventHandler(object sender, System.EventArgs e)
        {
            PainterStatus.OpenFileDirectory = null;
            PainterStatus.ImageHeight = SideMenuStatus.GetHeightOfNewImage;
            PainterStatus.ImageWidth = SideMenuStatus.GetWidthOfNewImage;
            PainterStatus.Initialize();
        }

        private void SaveFileChangedEventHandler(object sender, System.EventArgs e)
        {
            PainterStatus.SavePicture(SideMenuStatus.SaveFileDirectory);
        }

        private void OpenFileChangedEventHandler(object sender, System.EventArgs e)
        {
            PainterStatus.OpenFileDirectory = SideMenuStatus.OpenFileDirectory;
            //PainterStatus.ImageHeight = SideMenuStatus.GetHeightOfNewImage;
            //PainterStatus.ImageWidth = SideMenuStatus.GetWidthOfNewImage;
            PainterStatus.Initialize();
        }

        //private void ColorChangedEventHandler(object sender, System.EventArgs e)
        //{
        //    //throw new System.NotImplementedException();
        //}

        //private void BrushChangedEventHandler(object sender, System.EventArgs e)
        //{
        //    //throw new System.NotImplementedException();
        //}
    }
}
