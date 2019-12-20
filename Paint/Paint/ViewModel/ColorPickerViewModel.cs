using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Paint.ViewModel
{
    public class ColorPickerViewModel : BindableBase
    {
        #region Events
        public event System.EventHandler ColorPickerClosed;
        #endregion

        #region Events Realization
        protected virtual void OnColorChanged()
        {
            ColorPickerClosed?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Properties
        private Visibility _changeVisibilityOfPicker;

        private Color _selectedColor;
        #endregion

        #region Properties Realization
        public Visibility ChangeVisibilityOfPicker
        {
            get
            {
                return _changeVisibilityOfPicker;
            }
            set
            {
                _changeVisibilityOfPicker = value;
                RaisePropertyChanged("ChangeVisibilityOfPicker");
            }
        }

        public Color SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (_selectedColor != value)
                {
                    _selectedColor = value;
                    RaisePropertyChanged("SelectedColor");
                }
            }
        }
        #endregion

        #region Commands
        private ICommand _closeColorPicker;
        #endregion

        #region Commands Realization
        public ICommand CloseColorPicker => _closeColorPicker ?? (_closeColorPicker =
            new DelegateCommand(delegate ()
            {
                ChangeVisibilityOfPicker = Visibility.Collapsed;
                OnColorChanged();
            }));
        #endregion

        public ColorPickerViewModel()
        {
            ChangeVisibilityOfPicker = Visibility.Collapsed;
        }
    }
}
