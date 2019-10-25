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
        //public ColorPickerViewModel ColorPickerStatus { get; set; }

        //private Visibility _changeVisibilityOfMenu;

        private ICommand _openMenu;
        //private ICommand _openColorPicker;
        //private ICommand _closeMenu;

        public ICommand OpenMenu => _openMenu ?? (_openMenu = new RelayCommand(obj =>
        {
            SideMenuStatus.ChangeVisibilityOfMenu = Visibility.Visible;
        }));
        //public ICommand OpenColorPicker => _openColorPicker ?? (_openColorPicker = new RelayCommand(obj =>
        //{
        //    ColorPickerStatus.ChangeVisibilityOfPicker = Visibility.Visible;
        //}));

        //public ICommand CloseMenu => _closeMenu ?? (_closeMenu = new RelayCommand<object>(obj =>
        //{
        //    //ChangeVisibilityOfMenu = Visibility.Collapsed;
        //}));

        //public Visibility ChangeVisibilityOfMenu
        //{
        //    get
        //    {
        //        return this._changeVisibilityOfMenu;
        //    }
        //    set
        //    {
        //        _changeVisibilityOfMenu = value;
        //        OnPropertyChanged("ChangeVisibilityOfMenu");
        //    }
        //}

        public MainWindowViewModel()
        {
            SideMenuStatus = new SideMenuViewModel();
            BrushesBarStatus = new BrushesBarViewModel();
            //ColorPickerStatus = new ColorPickerViewModel();
        }
    }
}
