using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint.ViewModel
{
    public class LayerBarViewModel : BindableBase
    {
        public event System.EventHandler ItemIndexChanged;
        protected virtual void OnItemIndexChanged()
        {
            ItemIndexChanged?.Invoke(this, EventArgs.Empty);
        }

        public event System.EventHandler ItemChanged;
        protected virtual void OnItemChanged()
        {
            ItemChanged?.Invoke(this, EventArgs.Empty);
        }

        public event System.EventHandler AddItem;
        protected virtual void AddItemEvent()
        {
            AddItem?.Invoke(this, EventArgs.Empty);
        }

        public event System.EventHandler DeleteItem;
        protected virtual void DeleteItemEvent()
        {
            DeleteItem?.Invoke(this, EventArgs.Empty);
        }

        public ObservableCollection<Item> Items { get; set; }
        public int NumberOfActivatedLayers { get; private set; }
        private int Counter = 1;

        private int _itemIndex;
        public int ItemIndex
        {
            get => _itemIndex;
            set
            {
                _itemIndex = value;
                OnItemIndexChanged();
                RaisePropertyChanged("ItemIndex");
            }
        }

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

        private string _buttonText;
        public string ButtonText
        {
            get => _buttonText;
            set
            {
                _buttonText = value;
                RaisePropertyChanged("ButtonText");
            }
        }

        private bool IsExtended;
        private ICommand _extendBar;
        public ICommand ExtendBar => _extendBar ?? (_extendBar = new DelegateCommand(delegate ()
        {
            if (!IsExtended)
            {
                IsExtended = true;
                LayerBarHeight = 100;
                ItemImageSize = 70;
                ButtonText = "Уменьшить";
            }
            else
            {
                IsExtended = false;
                LayerBarHeight = 50;
                ItemImageSize = 24;
                ButtonText = "Увеличить";
            }
        }));

        private ICommand _addCommand;
        public ICommand AddCommand => _addCommand ?? (_addCommand = new DelegateCommand(delegate ()
        {
            AddItemEvent();
        }));

        private ICommand _deleteCommand;
        public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new DelegateCommand(delegate ()
        {
            DeleteItemEvent();
        }));

        public int GetNumberOfActivatedLayers()
        {
            int result = 0;
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].IsCheckedElement)
                {
                    result++;
                }
            }
            return result;
        }

        public void AddItemIntoCollection(WriteableBitmap writeableBitmap)
        {
            Items.Add(new Item(writeableBitmap, Counter++, false));
            Items[Items.Count - 1].ItemChanged += LayerBarViewModel_ItemChanged;
        }

        public void AddItemIntoCollection(WriteableBitmap writeableBitmap, bool isChecked)
        {
            if (isChecked) NumberOfActivatedLayers++;
            Items.Add(new Item(writeableBitmap, Counter++, isChecked));
            Items[Items.Count - 1].ItemChanged += LayerBarViewModel_ItemChanged;
        }

        public void DeleteItemFromCollection(int index)
        {
            Items[index].ItemChanged -= LayerBarViewModel_ItemChanged;
            Items.RemoveAt(index);
            Counter = 1;
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].LabelText = Counter++.ToString();
            }
        }

        public LayerBarViewModel()
        {
            Items = new ObservableCollection<Item>();
            LayerBarHeight = 50;
            ItemImageSize = 24;
            ButtonText = "Увеличить";
            ItemIndex = 0;
        }

        public void Clear()
        {
            Items.Clear();
            Counter = 1;
        }

        private void LayerBarViewModel_ItemChanged(object sender, EventArgs e)
        {
            NumberOfActivatedLayers = GetNumberOfActivatedLayers();
            OnItemChanged();
        }
    }

    public class Item : BindableBase
    {
        public event System.EventHandler ItemChanged;
        protected virtual void OnItemChanged()
        {
            ItemChanged?.Invoke(this, EventArgs.Empty);
        }

        private ImageSource _imageElementSource;
        public ImageSource ImageElementSource
        {
            get => _imageElementSource;
            set
            {
                _imageElementSource = value;
                RaisePropertyChanged("ImageElementSource");
            }
        }

        public bool _isCheckedElement;
        public bool IsCheckedElement
        {
            get => _isCheckedElement;
            set
            {
                _isCheckedElement = value;
                OnItemChanged();
                RaisePropertyChanged("IsCheckedElement");
            }
        }

        private int _position;
        public int Position
        {
            get => _position;
            set
            {
                _position = value;
                LabelText =  _position.ToString();
            }
        }

        private string _labelText;
        public string LabelText
        {
            get => _labelText;
            set
            {
                _labelText = value;
                RaisePropertyChanged("LabelText");
            }
        }

        public Item(WriteableBitmap writeableBitmap, int position, bool isCheckBoxTrue) : this()
        {
            ImageElementSource = writeableBitmap.Clone();
            Position = position;
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
            ItemImageSize = 24;
        }
    }
}
