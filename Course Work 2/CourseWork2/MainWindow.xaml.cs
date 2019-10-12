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
            //Test test = new Test(@"Hans Zimmer - Time (Cyberdesign Remix).mp3");
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.MinHeight = 480;
            Application.Current.MainWindow.MinWidth = 720;
            MousePositionInMainPictureBox.Visibility = Visibility.Hidden;
            MainPictureBox_SourceUpdated(null, null);
            //WorkSpace.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            //WorkSpace.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //StatusBar.Width = Application.Current.MainWindow.ActualWidth;
            if (WorkSpace.ActualHeight < MainPictureBox.ActualHeight)
            {
                WorkSpace.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            }
            else
            {
                WorkSpace.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            }
            if (WorkSpace.ActualWidth < MainPictureBox.ActualWidth)
            {
                WorkSpace.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            }
            else
            {
                WorkSpace.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            }
        }

        
    }
}
