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
            
            //MousePositionInMainPictureBox.Visibility = Visibility.Visible;
        }

        private void MainPictureBox_MouseLeave(object sender, MouseEventArgs e)
        {
            //MousePositionInMainPictureBox.Visibility = Visibility.Hidden;
        }

        private void MainPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            //MousePositionInMainPictureBox.Content = $"{Convert.ToInt32(e.GetPosition(null).X - 5)} x {Convert.ToInt32(e.GetPosition(null).Y - 100)} пкс";
        }

        private void MainPictureBox_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            StatusBarSizeOfImage.Content = $"{Convert.ToInt32(WorkSpace.Source.Width)} x {Convert.ToInt32(WorkSpace.Source.Height)} пкс";
        }
    }
}
