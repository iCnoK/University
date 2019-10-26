using Paint.ViewModel;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paint.View
{
    public partial class ColorPicker : UserControl
    {
        public ColorPicker()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ColorPickerViewModel viewModel = this.DataContext as ColorPickerViewModel;

            if (colorCanvas.SelectedColor != null)
            {
                System.Windows.Media.Color mediacolor = (System.Windows.Media.Color)colorCanvas.SelectedColor;

                viewModel.SelectedColor = System.Drawing.Color.FromArgb(
                    mediacolor.A, mediacolor.R, mediacolor.G, mediacolor.B);
            }
            else
            {
                viewModel.SelectedColor = Color.Transparent;
            }
        }
    }
}
