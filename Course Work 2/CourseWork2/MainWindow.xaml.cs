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
using CourseWork2.AudioParser;

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
            Application.Current.MainWindow.MinHeight = MainPictureBox.Source.Height + 20;
            Application.Current.MainWindow.MinWidth = MainPictureBox.Source.Width + 20;
        }
    }
}
