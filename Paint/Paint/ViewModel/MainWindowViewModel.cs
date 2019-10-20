using Paint.Utility;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Input;

namespace Paint.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        private Visibility _changeVisibilityOfMenu;

        private ICommand _openMenu;
        private ICommand _closeMenu;

        public ICommand OpenMenu => _openMenu ?? (_openMenu = new RelayCommand(obj =>
        {
            ChangeVisibilityOfMenu = Visibility.Visible;
        }));

        public ICommand CloseMenu => _closeMenu ?? (_closeMenu = new RelayCommand<object>(obj =>
        {
            ChangeVisibilityOfMenu = Visibility.Collapsed;
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

        public MainWindowViewModel()
        {
            ChangeVisibilityOfMenu = Visibility.Collapsed;
        }
    }
}
