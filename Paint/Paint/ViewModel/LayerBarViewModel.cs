using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint.ViewModel
{
    public class LayerBarViewModel : BindableBase
    {
        public event System.EventHandler ItemsChanged;

        protected virtual void OnItemsChanged()
        {
            ItemsChanged?.Invoke(this, EventArgs.Empty);
        }

        public ObservableCollection<Item> Items { get; set; }

        private int _layerBarHeight;
        public int LayerBarHeight
        {
            get => _layerBarHeight;
            set
            {
                _layerBarHeight = value;
                RaisePropertyChanged("LayerBarHeight");
            }
        }

        //private int _itemImageSize;
        public int ItemImageSize
        {
            get => Items[0].ItemImageSize;
            set
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    Items[i].ItemImageSize = value;
                }
            }
        }

        private ICommand _extendBar;
        public ICommand ExtendBar => _extendBar ?? (_extendBar = new DelegateCommand(delegate ()
        {
            LayerBarHeight = 100;
            ItemImageSize = 70;
        }));

        private ICommand _reduceBar;
        public ICommand ReduceBar => _reduceBar ?? (_reduceBar = new DelegateCommand(delegate ()
        {
            LayerBarHeight = 50;
            ItemImageSize = 0;
        }));



        public LayerBarViewModel()
        {
            Items = new ObservableCollection<Item>();
            LayerBarHeight = 50;
            ItemImageSize = 0;
        }
    }

    public class Item : BindableBase
    {
        public ImageSource ImageElementSource { get; set; }
        public bool IsCheckedElement { get; set; }

        public Item(WriteableBitmap writeableBitmap, bool isCheckBoxTrue)
        {
            ImageElementSource = writeableBitmap.Clone();
            IsCheckedElement = isCheckBoxTrue;
        }

        private int _itemImageSize;
        public int ItemImageSize
        {
            get => _itemImageSize;
            set
            {
                _itemImageSize = value;
                RaisePropertyChanged("ItemImageSize");
            }
        }

        public Item()
        {
            ItemImageSize = 0;
        }
    }
}
