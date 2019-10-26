using Paint.Utility;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Input;

namespace Paint.ViewModel
{
    public class MainWindowViewModel : OnPropertyChangedClass
    {
        public SideMenuViewModel SideMenuStatus { get; set; }
        public BrushesBarViewModel BrushesBarStatus { get; set; }

        private ICommand _openMenu;

        public ICommand OpenMenu => _openMenu ?? (_openMenu = new RelayCommand(obj =>
        {
            SideMenuStatus.ChangeVisibilityOfMenu = Visibility.Visible;
        }));

        public MainWindowViewModel()
        {
            SideMenuStatus = new SideMenuViewModel();
            BrushesBarStatus = new BrushesBarViewModel();
            BrushesBarStatus.BrushChanged += BrushChangedEventHandler;
            BrushesBarStatus.ColorChanged += ColorChangedEventHandler;
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
