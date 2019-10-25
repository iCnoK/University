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
        //private readonly SideMenuViewModel sideMenuViewModel = new SideMenuViewModel();


        //public delegate void CreateCommandUsedHandler(string sampleParam);

        //public event CreateCommandUsedHandler CreateCommandUsed;

        //public void RaiseCreateCommandUsed
        //{
        //    if (CreateCommandUsed != null)
        //    {

        //    }

        //}

        public SideMenu()
        {
            InitializeComponent();
            //DataContext = sideMenuViewModel;
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            SideMenuViewModel viewModel = this.DataContext as SideMenuViewModel;
            viewModel.CloseMenu.Execute(null);
        }

        //public ICommand CloseMenu
        //{
        //    get { return (ICommand)GetValue(CloseMenuProperty); }
        //    set { SetValue(CloseMenuProperty, value); }
        //}
        //public static readonly DependencyProperty CloseMenuProperty =
        //    DependencyProperty.Register("CloseMenu", typeof(ICommand), typeof(SideMenu));
    }
}
