using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint.ViewModel
{
    public class LayerBarViewModel : BindableBase
    {
        public ObservableCollection<Item> Items { get; set; }

        public LayerBarViewModel()
        {
            Items = new ObservableCollection<Item>();
            BitmapImage bitmapImage = new BitmapImage(new Uri(@"Z:\GitHub Repositories\University\Paint\Paint\Utility\Resources\star_wars_dart_vejder_art_105284_1920x1080.jpg"));
            bitmapImage.CreateOptions = BitmapCreateOptions.None;
            WriteableBitmap resultBitmap = new WriteableBitmap(bitmapImage);
            Items.Add(new Item(resultBitmap, true));
            Items.Add(new Item(resultBitmap, false));
        }
    }

    public class Item
    {
        Image Image { get; set; }
        CheckBox CheckBox { get; set; }

        public Item(WriteableBitmap writeableBitmap, bool isCheckBoxTrue)
        {
            Image = new Image();
            CheckBox = new CheckBox();
            //Image.Source = writeableBitmap;
            Image.Source = writeableBitmap.Clone();
            CheckBox.IsChecked = isCheckBoxTrue;
        }

        public Item()
        {

        }
    }
}
