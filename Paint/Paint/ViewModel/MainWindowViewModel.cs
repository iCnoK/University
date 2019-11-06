using Paint.Utility;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Input;

namespace Paint.ViewModel
{
    public class MainWindowViewModel : OnPropertyChangedClass
    {
        private DataManager DataManager = new DataManager();

        public SideMenuViewModel SideMenuStatus { get; set; }
        public BrushesBarViewModel BrushesBarStatus { get; set; }
        public PainterViewModel PainterStatus { get; set; }

        private ICommand _openMenu;

        public ICommand OpenMenu => _openMenu ?? (_openMenu = new RelayCommand(obj =>
        {
            SideMenuStatus.ChangeVisibilityOfMenu = Visibility.Visible;
        }));

        public MainWindowViewModel()
        {
            SideMenuStatus = new SideMenuViewModel();
            BrushesBarStatus = new BrushesBarViewModel(DataManager);
            PainterStatus = new PainterViewModel(DataManager);

            SideMenuStatus.OpenFileChanged += OpenFileChangedEventHandler;
            SideMenuStatus.SaveFileChanged += SaveFileChangedEventHandler;
            SideMenuStatus.ImageSizeChanged += ImageSizeChangedEventHandler;

            BrushesBarStatus.BrushChanged += BrushChangedEventHandler;
            BrushesBarStatus.ColorChanged += ColorChangedEventHandler;
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
            //throw new System.NotImplementedException();
        }

        private void OpenFileChangedEventHandler(object sender, System.EventArgs e)
        {
            PainterStatus.OpenFileDirectory = SideMenuStatus.OpenFileDirectory;
            //PainterStatus.ImageHeight = SideMenuStatus.GetHeightOfNewImage;
            //PainterStatus.ImageWidth = SideMenuStatus.GetWidthOfNewImage;
            PainterStatus.Initialize();
        }

        private void ColorChangedEventHandler(object sender, System.EventArgs e)
        {
            //throw new System.NotImplementedException();
        }

        private void BrushChangedEventHandler(object sender, System.EventArgs e)
        {
            //throw new System.NotImplementedException();
        }
    }
}
