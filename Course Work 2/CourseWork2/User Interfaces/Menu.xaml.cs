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

namespace CourseWork2.User_Interfaces
{
    public partial class Menu : UserControl
    {
        public Menu()
        {
            InitializeComponent();
            this.Width = 300;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            this.Width = 300;
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            this.Width = 300;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.Width = 300;
        }

        private void SaveAsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Width = 300;
        }

        private void ParametersButton_Click(object sender, RoutedEventArgs e)
        {
            this.Width = 600;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
