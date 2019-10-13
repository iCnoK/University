
using System;
using System.Collections.Generic;
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
//<Image x:Name="MainPictureBox" Margin="10,100,10,10" Source="C:\Users\Andrey\Desktop\BoardingPass_MyNameOnMars2020.png" RenderTransformOrigin="0.5,0.5" HorizontalAlignment="Left" VerticalAlignment="Top" StretchDirection="DownOnly" Stretch="None" />
namespace MainGUI
{
    public partial class MainWindow : Window
    {
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
            //ScrollViewer.Height = WorkSpace.Source.Height;
            //ScrollViewer.Width = WorkSpace.Source.Width;
            
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //StatusBar.Width = Application.Current.MainWindow.ActualWidth;
            //if (ScrollViewer.ActualHeight < WorkSpace.ActualHeight)
            //{
            //    ScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            //}
            //else
            //{
            //    ScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            //}
            //if (ScrollViewer.ActualWidth < WorkSpace.ActualWidth)
            //{
            //    ScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            //}
            //else
            //{
            //    ScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            //}
        }

        private void ImageScale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SliderLabel.Content = ImageScale.Value + "%";
            //WorkSpace.Stretch = Stretch.Fill;
            WorkSpace.Height = WorkSpace.Source.Height / 100 * ImageScale.Value;
            WorkSpace.Width = WorkSpace.Source.Width / 100 * ImageScale.Value;
            //WorkSpace.HorizontalAlignment = HorizontalAlignment.Center;
            //WorkSpace.VerticalAlignment = VerticalAlignment.Center;
        }
    }
}
