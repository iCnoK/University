using Microsoft.Win32;
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

namespace UserInterface.SideMenu
{
    public partial class Menu : UserControl
    {
        public bool IsInputFileEmpty
        {
            get
            {
                if (InputFileName == null && InputFileNameOnly == null)
                {
                    return true;
                }
                return false;
            }
        }
        public bool IsOutputFileEmpty
        {
            get
            {
                if (OutputFileName == null && OutputFileNameOnly == null)
                {
                    return true;
                }
                return false;
            }
        }
        public string InputFileName { get; private set; } = null;
        public string InputFileNameOnly { get; private set; } = null;
        public string OutputFileName { get; private set; } = null;
        public string OutputFileNameOnly { get; private set; } = null;

        public Menu()
        {
            InitializeComponent();
            this.Width = 300;
        }

        private const string Filter = "Файлы рисунков (*.jpg,*.png,*.jpeg)|*.jpg;*.png;*.jpeg";

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog();
            Application.Current.MainWindow.
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void SaveAsButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog();
        }

        private void ParametersButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SaveFileDialog()
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = Filter;
            fileDialog.DefaultExt = "jpg";
            fileDialog.AddExtension = true;
            fileDialog.FileName = "untitled.jpg";
            fileDialog.ShowDialog();
            OutputFileName = fileDialog.FileName;
            OutputFileNameOnly = fileDialog.SafeFileName;
        }

        private void OpenFileDialog()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Выберите файл";
            fileDialog.Filter = Filter;
            fileDialog.CheckFileExists = true;
            fileDialog.ShowDialog();
            InputFileName = fileDialog.FileName;
            InputFileNameOnly = fileDialog.SafeFileName;
        }
    }
}
