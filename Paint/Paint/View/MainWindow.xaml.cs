using Paint.Utility.MVVM_Support;
using Paint.ViewModel;
using System;
using System.Windows;

namespace Paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private readonly MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
            (this.DataContext as MainWindowViewModel).MessageBoxRequest += new EventHandler<MvvmMessageBoxEventArgs>(MyView_MessageBoxRequest);
        }

        void MyView_MessageBoxRequest(object sender, MvvmMessageBoxEventArgs e)
        {
            e.Show();
        }
    }
}
