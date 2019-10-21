using Paint.Utility;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Input;

namespace Paint.ViewModel
{
    public class SideMenuViewModel : BindableBase
    {
        private Visibility _changeVisibilityOfMenu;

        private int _width;

        private ICommand _closeMenu;
        private ICommand _closeApplication;
        private ICommand _createNewPicture;

        private ICommand _openParameters;

        public ICommand CloseMenu => _closeMenu ?? (_closeMenu = new RelayCommand(obj =>
        {
            Width = 300;
            ChangeVisibilityOfMenu = Visibility.Collapsed;
        }));
        public ICommand CloseApplication => _closeApplication ?? (_closeApplication =
            new RelayCommand(obj =>
            {
                Application.Current.Shutdown();
            }));
        public ICommand CreateNewPicture => _createNewPicture ?? (_createNewPicture =
            new RelayCommand(obj =>
            {
                Width = 300;
                //TODO => здесь должен быть создатель пустого изображения
            }));


        public ICommand OpenParameters => _openParameters ?? (_openParameters =
            new RelayCommand(onj =>
            {
                Width = 600;
            }));



        public Visibility ChangeVisibilityOfMenu
        {
            get
            {
                return _changeVisibilityOfMenu;
            }
            set
            {
                _changeVisibilityOfMenu = value;
                RaisePropertyChanged("ChangeVisibilityOfMenu");
            }
        }
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                RaisePropertyChanged("Width");
            }
        }
        

        public SideMenuViewModel()
        {
            ChangeVisibilityOfMenu = Visibility.Collapsed;
            Width = 300;
        }
    }
}
