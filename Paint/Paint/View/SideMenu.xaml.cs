using Paint.ViewModel;
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

namespace Paint.View
{
    public partial class SideMenu : UserControl
    {
        public SideMenu()
        {
            InitializeComponent();
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            SideMenuViewModel viewModel = this.DataContext as SideMenuViewModel;
            viewModel.CloseMenu.Execute(null);
        }
    }
}
