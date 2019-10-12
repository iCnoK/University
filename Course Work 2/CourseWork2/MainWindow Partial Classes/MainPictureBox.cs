using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace MainGUI
{
    public partial class MainWindow
    {
        private void MainPictureBox_MouseEnter(object sender, MouseEventArgs e)
        {
            
            MousePositionInMainPictureBox.Visibility = Visibility.Visible;
        }

        private void MainPictureBox_MouseLeave(object sender, MouseEventArgs e)
        {
            MousePositionInMainPictureBox.Visibility = Visibility.Hidden;
        }

        private void MainPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            
            MousePositionInMainPictureBox.Content = $"{e.GetPosition(null).X} x {e.GetPosition(null).Y}\tпкс";
        }

        private void MainPictureBox_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            StatusBarSizeOfImage.Content = $"{MainPictureBox.ActualWidth} x {MainPictureBox.ActualHeight}\tпкс";
        }
    }
}
