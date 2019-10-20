using Paint.Utility;
using Paint.View;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Paint.ViewModel
{
    public class MainWindowViewModel : OnPropertyChangedClass
    {
        private Visibility _changeVisibilityOfMenu;

        private ICommand _openMenu;
        //private ICommand _closeMenu;

        public ICommand OpenMenu => _openMenu ?? (_openMenu = new RelayCommand(obj =>
        {
            ChangeVisibilityOfMenu = Visibility.Visible;
        }));
        //public ICommand CloseMenu => _closeMenu ?? (_closeMenu = new RelayCommand(obj =>
        //{
        //    ChangeVisibilityOfMenu = Visibility.Hidden;
        //}));
        
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
            ChangeVisibilityOfMenu = Visibility.Hidden;
        }




        //public ICommand Command
        //{
        //    get 
        //    {
        //        return (ICommand)GetValue(CommandProperty);
        //    }
        //    set 
        //    { 
        //        SetValue(CommandProperty, value); 
        //    }
        //}
        //private ICommand _closeMenu;
        //public ICommand CloseMenu;
        //{
        //    get
        //    {
        //        return (ICommand)CloseMenu;
        //    }
        //    set
        //    {
        //        SetValue(CommandProperty, value);
        //    }
        //}
        //public DependencyProperty CloseMenu =
        //    DependencyProperty.Register("CloseMenu", typeof(ICommand), typeof(SideMenu));



    }
}
