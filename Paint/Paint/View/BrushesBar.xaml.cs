using Paint.Utility.Enums;
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
    /// <summary>
    /// Interaction logic for BrushesBar.xaml
    /// </summary>
    public partial class BrushesBar : UserControl
    {
        public BrushesBar()
        {
            InitializeComponent();
        }



        //public BrushType BrushType
        //{
        //    get { return (BrushType)GetValue(BrushTypeProperty); }
        //    set { SetValue(BrushTypeProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for BrushType.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty BrushTypeProperty =
        //    DependencyProperty.Register("BrushType", typeof(int), typeof(BrushesBar), new PropertyMetadata(0));



        private void NumericOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text);
        }

        private static bool IsTextNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            return reg.IsMatch(str);
        }
    }
}
