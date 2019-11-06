﻿using Paint.Utility;
using Paint.Utility.Enums;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Drawing;
//using System.Windows.Media;

namespace Paint.ViewModel
{
    public class BrushesBarViewModel : BindableBase
    {
        public ColorPickerViewModel ColorPickerStatus { get; set; }

        private DataManager DataManager { get; set; }


        #region Misc
        public BrushType LastChangedBrush { get; private set; }
        
        public ObservableCollection<System.Windows.Media.SolidColorBrush> customSolidBrushes { get; set; } = 
            new ObservableCollection<System.Windows.Media.SolidColorBrush>();
        
        private int? _customColorsSelectedIndex;
        #endregion

        #region Events
        public event System.EventHandler BrushChanged;
        protected virtual void OnBrushChanged()
        {
            BrushChanged?.Invoke(this, EventArgs.Empty);
        }
        
        public event System.EventHandler ColorChanged;
        protected virtual void OnColorChanged()
        {
            ColorChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Properties
        private Visibility _changeVisibilityOfBar;
        public Visibility ChangeVisibilityOfBar
        {
            get
            {
                return _changeVisibilityOfBar;
            }
            set
            {
                _changeVisibilityOfBar = value;
                RaisePropertyChanged("ChangeVisibilityOfBar");
            }
        }
        
        private Visibility _opacityVisibility;
        public Visibility OpacityVisibility
        {
            get
            {
                return _opacityVisibility;
            }
            set
            {
                _opacityVisibility = value;
                RaisePropertyChanged("OpacityVisibility");
            }
        }
        
        private Visibility _widthVisibility;
        public Visibility WidthVisibility
        {
            get
            {
                return _widthVisibility;
            }
            set
            {
                _widthVisibility = value;
                RaisePropertyChanged("WidthVisibility");
            }
        }

        private BrushType _brushType;
        public BrushType BrushType
        {
            get
            {
                return _brushType;
            }
            set
            {
                if (_brushType != value)
                {
                    _brushType = value;
                    RaisePropertyChanged("BrushType");
                    OnBrushChanged();
                }
            }
        }

        private int _widthSliderValue;
        public int WidthSliderValue
        {
            get
            {
                return _widthSliderValue;
            }
            set
            {
                _widthSliderValue = value;
                RaisePropertyChanged("WidthSliderValue");
                DataManager.SetCurrentWidthSliderValue(LastChangedBrush, value);
                //sliderValueHolder[LastChangedBrush, SliderMode.WIDTH] = value;
            }
        }

        private int _opacitySliderValue;
        public int OpacitySliderValue
        {
            get
            {
                return _opacitySliderValue;
            }
            set
            {
                _opacitySliderValue = value;
                RaisePropertyChanged("OpacitySliderValue");
                DataManager.SetCurrentOpacitySliderValue(LastChangedBrush, value);
                //sliderValueHolder[LastChangedBrush, SliderMode.OPACITY] = value;
            }
        }

        private int _widthSliderMinimum;
        public int WidthSliderMinimum
        {
            get => _widthSliderMinimum;
            set
            {
                _widthSliderMinimum = value;
                RaisePropertyChanged("WidthSliderMinimum");
            }
        }

        private int _widthSliderMaximum;
        public int WidthSliderMaximum
        {
            get => _widthSliderMaximum;
            set
            {
                _widthSliderMaximum = value;
                RaisePropertyChanged("WidthSliderMaximum");
            }
        }

        private Color _currentSelectedColor;
        public Color CurrentSelectedColor
        {
            get => _currentSelectedColor;
            set
            {
                if (_currentSelectedColor != value)
                {
                    _currentSelectedColor = value;
                    RaisePropertyChanged("CurrentSelectedColor");
                    OnColorChanged();
                }
            }
        }

        private bool _buttonsIsEnabled;
        public bool ButtonsIsEnabled
        {
            get => _buttonsIsEnabled;
            set
            {
                _buttonsIsEnabled = value;
                RaisePropertyChanged("ButtonsIsEnabled");
            }
        }
        #endregion

        #region Commands
        private ICommand _setBrush;
        public ICommand SetBrush => _setBrush ?? (_setBrush =
            new RelayCommand(obj =>
            {
                if (obj != null)
                {
                    BrushType = (BrushType)obj;
                    LastChangedBrush = (BrushType)obj;

                    MaxMin temp = DataManager.GetWidthSliderMinMax(LastChangedBrush);
                    WidthSliderMaximum = temp.Max;
                    WidthSliderMinimum = temp.Min;
                    WidthSliderValue = DataManager.GetCurrentWidthSliderValue(LastChangedBrush);
                    OpacitySliderValue = DataManager.GetCurrentOpacitySliderValue(LastChangedBrush);
                    //WidthSliderMaximum = sliderValueHolder[(BrushType)obj].Maximum;
                    //WidthSliderMinimum = sliderValueHolder[(BrushType)obj].Minimum;
                    //WidthSliderValue = sliderValueHolder[(BrushType)obj].WidthValue;
                    //OpacitySliderValue = sliderValueHolder[(BrushType)obj].OpacityValue;
                    if (LastChangedBrush == BrushType.FILL)
                    {
                        BlockAllSliders();
                    }
                    else if (LastChangedBrush == BrushType.OILBRUSH || LastChangedBrush == BrushType.ERASER) 
                    {
                        BlockSlider();
                    }
                    else
                    {
                        UnblockSliders();
                    }
                }
            }));

        private ICommand _setDefaultColor;
        public ICommand SetDefaultColor => _setDefaultColor ?? (_setDefaultColor = 
            new RelayCommand(obj =>
            {
                if (obj != null)
                {
                    ColorConverter converter = new ColorConverter();
                    
                    CurrentSelectedColor = (Color)converter.ConvertFromString(obj.ToString());
                    _customColorsSelectedIndex = null;
                    ButtonsIsEnabled = false;
                }
            }));

        private ICommand _setCustomColor;
        public ICommand SetCustomColor => _setCustomColor ?? (_setCustomColor =
            new RelayCommand(obj =>
            {
                if (obj != null)
                {
                    _customColorsSelectedIndex = Convert.ToInt32(obj.ToString());
                    ButtonsIsEnabled = true;

                }
            }));

        private ICommand _openColorPicker;
        public ICommand OpenColorPicker => _openColorPicker ?? (_openColorPicker =
            new RelayCommand(obj =>
            {
                ColorPickerStatus.ChangeVisibilityOfPicker = Visibility.Visible;
            }));
        #endregion

        public BrushesBarViewModel() { }

        public BrushesBarViewModel(DataManager dataManager)
        {
            DataManager = dataManager;
            ColorPickerStatus = new ColorPickerViewModel();
            SetBrush.Execute(BrushType.MARKER);
            for (int i = 0; i < 12; i++)
            {
                customSolidBrushes.Add(new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Colors.Transparent));
            }
            ButtonsIsEnabled = false;
            ColorPickerStatus.ColorPickerClosed += ColorPickerClosedEventHandler;
        }

        private void ColorPickerClosedEventHandler(object sender, EventArgs e)
        {
            CurrentSelectedColor = ColorPickerStatus.SelectedColor;

            customSolidBrushes[(int)_customColorsSelectedIndex].Color = 
                CustomColorConverter.ConvertFromSDCToSWMC(ColorPickerStatus.SelectedColor);
        }

        #region SliderManage
        private void BlockSlider()
        {
            OpacityVisibility = Visibility.Collapsed;
        }

        private void BlockAllSliders()
        {
            OpacityVisibility = Visibility.Collapsed;
            WidthVisibility = Visibility.Collapsed;
        }

        private void UnblockSliders()
        {
            OpacityVisibility = Visibility.Visible;
            WidthVisibility = Visibility.Visible;
        }
        #endregion
    }
}