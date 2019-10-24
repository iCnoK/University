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
using System.Windows.Media;

namespace Paint.ViewModel
{
    public class BrushesBarViewModel : BindableBase
    {
        #region Misc
        private Slider sliderValueHolder = new Slider();
        public BrushType LastChangedBrush { get; private set; }
        private ObservableCollection<Color> customColors { get; set; } = new ObservableCollection<Color>();
        #endregion

        #region Properties
        private Visibility _changeVisibilityOfBar;
        private Visibility _opacityVisibility;
        private Visibility _widthVisibility;

        private BrushType _brushType;

        private int _widthSliderValue;
        private int _opacitySliderValue;
        private int _widthSliderMinimum;
        private int _widthSliderMaximum;

        private int? _customColorsSelectedIndex;

        private Color _currentSelectedColor;

        private bool _buttonsIsEnabled;
        #endregion

        #region PropertiesRealization
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

        public BrushType BrushType
        {
            get
            {
                return _brushType;
            }
            set
            {
                _brushType = value;
                RaisePropertyChanged("BrushType");
            }
        }

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
                sliderValueHolder[LastChangedBrush, SliderInfoMode.WIDTH] = value;
            }
        }
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
                sliderValueHolder[LastChangedBrush, SliderInfoMode.OPACITY] = value;
            }
        }
        public int WidthSliderMinimum
        {
            get => _widthSliderMinimum;
            set
            {
                _widthSliderMinimum = value;
                RaisePropertyChanged("WidthSliderMinimum");
            }
        }
        public int WidthSliderMaximum
        {
            get => _widthSliderMaximum;
            set
            {
                _widthSliderMaximum = value;
                RaisePropertyChanged("WidthSliderMaximum");
            }
        }

        public Color CurrentSelectedColor
        {
            get => _currentSelectedColor;
            set
            {
                _currentSelectedColor = value;
                RaisePropertyChanged("CurrentSelectedColor");
            }
        }

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

        private ICommand _setDefaultColor;
        private ICommand _setCustomColor;

        private ICommand _addColorIntoCollection;
        private ICommand _removeColorFromCollection;
        #endregion

        #region CommandsRealization
        public ICommand SetBrush => _setBrush ?? (_setBrush =
            new RelayCommand(obj =>
            {
                if (obj != null)
                {
                    BrushType = (BrushType)obj;
                    LastChangedBrush = (BrushType)obj;
                    WidthSliderMaximum = sliderValueHolder[(BrushType)obj].Maximum;
                    WidthSliderMinimum = sliderValueHolder[(BrushType)obj].Minimum;
                    WidthSliderValue = sliderValueHolder[(BrushType)obj].WidthValue;
                    OpacitySliderValue = sliderValueHolder[(BrushType)obj].OpacityValue;
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
        public ICommand SetDefaultColor => _setDefaultColor ?? (_setDefaultColor = 
            new RelayCommand(obj =>
            {
                if (obj != null)
                {
                    CurrentSelectedColor = (Color)ColorConverter.ConvertFromString(obj.ToString());
                    _customColorsSelectedIndex = null;
                    ButtonsIsEnabled = false;
                }
            }));
        public ICommand SetCustomColor => _setCustomColor ?? (_setCustomColor =
            new RelayCommand(obj =>
            {
                if (obj != null)
                {
                    _customColorsSelectedIndex = Convert.ToInt32(obj.ToString());
                    ButtonsIsEnabled = true;
                }
            }));
        #endregion

        public BrushesBarViewModel()
        {
            SetBrush.Execute(BrushType.MARKER);
            for (int i = 0; i < 12; i++)
            {
                customColors.Add(new Color());
                customColors[i] = Colors.Transparent;
            }
            ButtonsIsEnabled = false;
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