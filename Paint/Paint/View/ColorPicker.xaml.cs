using Paint.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
                //System.Windows.Media.Color mediacolor = (System.Windows.Media.Color)colorCanvas.SelectedColor;

                //viewModel.SelectedColor = System.Drawing.Color.FromArgb(
                //    mediacolor.A, mediacolor.R, mediacolor.G, mediacolor.B);
                viewModel.SelectedColor = (Color)colorCanvas.SelectedColor;
            }
            else
            {
                viewModel.SelectedColor = Colors.Transparent;
            }
        }
    }
}
