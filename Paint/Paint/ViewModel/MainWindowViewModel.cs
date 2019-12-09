﻿using Paint.Utility;
using Paint.Utility.MVVM_Support;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Paint.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        public event EventHandler<MvvmMessageBoxEventArgs> MessageBoxRequest;
        private void ShowMessageBox(Action<MessageBoxResult> resultAction, string messageBoxText, string caption = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.None, MessageBoxOptions options = MessageBoxOptions.None)
        {
            this.MessageBoxRequest?.Invoke(this, new MvvmMessageBoxEventArgs(resultAction, messageBoxText, caption, button, icon, defaultResult, options));
        }
        
        private BrushParameters BrushParameters = new BrushParameters();

        public SideMenuViewModel SideMenuStatus { get; set; }
        public BrushesBarViewModel BrushesBarStatus { get; set; }
        public PainterViewModel PainterStatus { get; set; }

        private ICommand _openMenu;
        public ICommand OpenMenu => _openMenu ?? (_openMenu = new DelegateCommand(delegate ()
        {
            SideMenuStatus.ChangeVisibilityOfMenu = Visibility.Visible;
        }));

        private ICommand _undoChanges;
        public ICommand UndoChanges => _undoChanges ?? (_undoChanges = new DelegateCommand(delegate ()
        {
            PainterStatus.UndoChanges();
        }));

        private ICommand _openBrushConfigurator;
        public ICommand OpenBrushConfigurator => _openBrushConfigurator ?? (_openBrushConfigurator = new DelegateCommand(delegate ()
        {
            ShowMessageBox(ProcessTheAnswerOfMessageBox,
                "Вы уверены, что желаете закрыть PainD и открыть конфигуратор кистей?\nИзображение автоматически сохранится.",
                "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
        }));

        public void ProcessTheAnswerOfMessageBox(MessageBoxResult result)
        {
            if (result == MessageBoxResult.OK)
            {
                SideMenuStatus.SavePicture.Execute(null);
                Process.Start("BrushCreator.exe");
                Application.Current.Shutdown();
            }
        }

        public MainWindowViewModel()
        {
            SideMenuStatus = new SideMenuViewModel();
            BrushesBarStatus = new BrushesBarViewModel(BrushParameters);
            PainterStatus = new PainterViewModel(BrushParameters);

            SideMenuStatus.OpenFileChanged += OpenFileChangedEventHandler;
            SideMenuStatus.SaveFileChanged += SaveFileChangedEventHandler;
            SideMenuStatus.ImageSizeChanged += ImageSizeChangedEventHandler;

            //BrushesBarStatus.BrushChanged += BrushChangedEventHandler;
            //BrushesBarStatus.ColorChanged += ColorChangedEventHandler;
        }

        private void ImageSizeChangedEventHandler(object sender, System.EventArgs e)
        {
            PainterStatus.OpenFileDirectory = null;
            PainterStatus.ImageHeight = SideMenuStatus.GetHeightOfNewImage;
            PainterStatus.ImageWidth = SideMenuStatus.GetWidthOfNewImage;
            PainterStatus.Initialize();
        }

        private void SaveFileChangedEventHandler(object sender, System.EventArgs e)
        {
            PainterStatus.SavePicture(SideMenuStatus.SaveFileDirectory);
        }

        private void OpenFileChangedEventHandler(object sender, System.EventArgs e)
        {
            PainterStatus.OpenFileDirectory = SideMenuStatus.OpenFileDirectory;
            //PainterStatus.ImageHeight = SideMenuStatus.GetHeightOfNewImage;
            //PainterStatus.ImageWidth = SideMenuStatus.GetWidthOfNewImage;
            PainterStatus.Initialize();
        }

        //private void ColorChangedEventHandler(object sender, System.EventArgs e)
        //{
        //    //throw new System.NotImplementedException();
        //}

        //private void BrushChangedEventHandler(object sender, System.EventArgs e)
        //{
        //    //throw new System.NotImplementedException();
        //}
    }
}
