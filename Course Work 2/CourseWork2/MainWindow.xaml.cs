using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainGUI
{
    public partial class MainWindow : Window
    {
        UserInterface.SideMenu.Menu MainMenu = new UserInterface.SideMenu.Menu();

        //public string PathToFile { get; set; } = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.MinHeight = 720;
            Application.Current.MainWindow.MinWidth = 1280;
            MousePositionInMainPictureBox.Visibility = Visibility.Hidden;
            MainPictureBox_SourceUpdated(null, null);
            ScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            ScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            MainWindowBlurEffect.Radius = 0;

             




            //ScrollViewer.Height = WorkSpace.Source.Height;
            //ScrollViewer.Width = WorkSpace.Source.Width;

            //TEST.Height = WorkSpace.ActualHeight;
            //TEST.Width = WorkSpace.ActualWidth;
            
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainMenu.Height = Application.Current.MainWindow.ActualHeight - 35;
        }

        private void ImageScale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SliderLabel.Content = ImageScale.Value + "%";

            //WorkSpace.Stretch = Stretch.Fill;

            //WorkSpace.Height = WorkSpace.Source.Height / 100 * ImageScale.Value;
            //WorkSpace.Width = WorkSpace.Source.Width / 100 * ImageScale.Value;

            //WorkSpace.HorizontalAlignment = HorizontalAlignment.Center;
            //WorkSpace.VerticalAlignment = VerticalAlignment.Center;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Height = Application.Current.MainWindow.ActualHeight - 35;
            MainWindowBlurEffect.Radius = 5;
            AllControlsGrid.IsEnabled = false;
            if (MainMenu.IsLoaded)
            {
                MainMenu.Visibility = Visibility.Visible;
            }
            else
            {
                MenuPanel.Children.Add(MainMenu);
            }
            MainMenu.IsVisibleChanged += MainMenu_IsVisibleChanged;
        }

        private void MainMenu_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UserControl user = (UserControl)sender;
            if (user.Visibility == Visibility.Hidden)
            {
                MainWindowBlurEffect.Radius = 0;
                AllControlsGrid.IsEnabled = true;
                //todo очистка памяти для прошлого изображения
            }
        }

    }
}
