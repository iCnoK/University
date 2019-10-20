using Paint.Utility;
using System.Windows;
using System.Windows.Input;

namespace Paint.ViewModel
{
    public class SideMenuViewModel : OnPropertyChangedClass
    {
        private Visibility _changeVisibilityOfMenu;

        private ICommand _closeMenu;
        private ICommand _closeApplication;

        public ICommand CloseMenu => _closeMenu ?? (_closeMenu = new RelayCommand(obj =>
        {
            ChangeVisibilityOfMenu = Visibility.Collapsed;
        }));
        public ICommand CloseApplication => _closeApplication ?? (_closeApplication =
            new RelayCommand(obj =>
            {
                Application.Current.Shutdown();
            }));




        public Visibility ChangeVisibilityOfMenu
        {
            get
            {
                return this._changeVisibilityOfMenu;
            }
            set
            {
                _changeVisibilityOfMenu = value;
                OnPropertyChanged("ChangeVisibilityOfMenu");
            }
        }

        public SideMenuViewModel()
        {
            ChangeVisibilityOfMenu = Visibility.Collapsed;
        }
    }
}
