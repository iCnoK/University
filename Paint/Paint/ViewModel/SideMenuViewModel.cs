using Microsoft.Win32;
using Paint.Model.SideMenuControl;
using Paint.Utility;
using Prism.Mvvm;
using System;
using System.Windows;
using System.Windows.Input;

namespace Paint.ViewModel
{
    public class SideMenuViewModel : BindableBase
    {
        public int GetHeightOfNewImage
        {
            get => Convert.ToInt32(StringImageHeigth);
        }
        public int GetWidthOfNewImage
        {
            get => Convert.ToInt32(StringImageWidth);
        }


        #region Directories
        private string _openFileDirectory;
        private string _saveFileDirectory;

        public string OpenFileDirectory
        {
            get => _openFileDirectory;
            private set
            {
                _openFileDirectory = value;
            }
        }
        public string OpenFileName { get; private set; }
        public string SaveFileDirectory
        {
            get => _saveFileDirectory;
            private set
            {
                _saveFileDirectory = value;
                OnSaveFileChanged();
            }
        }
        public string SaveFileName { get; private set; }
        #endregion

        #region Events
        public event System.EventHandler OpenFileChanged;
        public event System.EventHandler SaveFileChanged;
        public event System.EventHandler ImageSizeChanged;
        #endregion

        #region Events Realization
        protected virtual void OnOpenFileChanged()
        {
            OpenFileChanged?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnSaveFileChanged()
        {
            SaveFileChanged?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnImageSizeChanged()
        {
            ImageSizeChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Properties
        private Visibility _changeVisibilityOfMenu;
        private Visibility _createBarVisibility;

        private int _width;

        private string _imageHeigth;
        private string _imageWidth;
        #endregion

        #region Properties Realization
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
        public Visibility CreateBarVisibility
        {
            get => _createBarVisibility;
            set
            {
                _createBarVisibility = value;
                RaisePropertyChanged("CreateBarVisibility");
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
        public string StringImageHeigth
        {
            get => _imageHeigth;
            set
            {
                _imageHeigth = value;
                RaisePropertyChanged("StringImageHeigth");
            }
        }
        public string StringImageWidth
        {
            get => _imageWidth;
            set
            {
                _imageWidth = value;
                RaisePropertyChanged("StringImageWidth");
            }
        }
        #endregion

        #region Commands
        private ICommand _closeMenu;
        private ICommand _closeApplication;
        private ICommand _createNewPicture;
        //private ICommand _openParameters;
        private ICommand _openNewPicture;
        private ICommand _savePicture;
        private ICommand _saveAsPicture;
        private ICommand _changeCreateBarVisibility;
        #endregion

        #region Commands Realization
        public ICommand CloseMenu => _closeMenu ?? (_closeMenu = new RelayCommand(obj =>
        {
            //Width = 300;
            ChangeVisibilityOfMenu = Visibility.Collapsed;
            CreateBarVisibility = Visibility.Collapsed;
        }));
        public ICommand CloseApplication => _closeApplication ?? (_closeApplication =
            new RelayCommand(obj =>
            {
                Application.Current.Shutdown();
            }));
        //public ICommand OpenParameters => _openParameters ?? (_openParameters =
        //    new RelayCommand(onj =>
        //    {
        //        //Width = 600;
        //    }));
        public ICommand CreateNewPicture => _createNewPicture ?? (_createNewPicture =
            new RelayCommand(obj =>
            {
                //Width = 300;
                //TODO => здесь должен быть создатель пустого изображения
                CreateBarVisibility = Visibility.Visible;
            }));
        public ICommand OpenNewPicture => _openNewPicture ?? (_openNewPicture =
            new RelayCommand(obj =>
            {
                CreateBarVisibility = Visibility.Collapsed;
                OpenFileDialog fileDialog = SideMenuModel.InitOpenFileDialog();
                fileDialog.ShowDialog();

                if (fileDialog.FileName != string.Empty)
                {
                    OpenFileDirectory = fileDialog.FileName;
                    OpenFileName = fileDialog.SafeFileName;
                    OnOpenFileChanged();
                }
            }));

        public ICommand SavePicture => _savePicture ?? (_savePicture =
            new RelayCommand(obj =>
            {
                CreateBarVisibility = Visibility.Collapsed;
                OnSaveFileChanged();
            }));
        public ICommand SaveAsPicture => _saveAsPicture ?? (_saveAsPicture =
            new RelayCommand(obj =>
            {
                CreateBarVisibility = Visibility.Collapsed;

                SaveFileDialog fileDialog = SideMenuModel.InitSaveFileDialog();
                fileDialog.ShowDialog();

                if (fileDialog.FileName != string.Empty)
                {
                    SaveFileDirectory = fileDialog.FileName;
                    SaveFileName = fileDialog.SafeFileName;
                    OnSaveFileChanged();
                }
            }));

        public ICommand ChangeCreateBarVisibility => _changeCreateBarVisibility ?? (_changeCreateBarVisibility =
            new RelayCommand(obj =>
            {
                CreateBarVisibility = Visibility.Collapsed;
                OnImageSizeChanged();
            }));
        #endregion

        public SideMenuViewModel()
        {
            ChangeVisibilityOfMenu = Visibility.Collapsed;
            CreateBarVisibility = Visibility.Collapsed;
            Width = 300;
            SaveFileDirectory = $"{Environment.CurrentDirectory}\\unknown.jpg";
        }
    }
}
