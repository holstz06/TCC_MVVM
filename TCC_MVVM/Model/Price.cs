using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC_MVVM.Model
{
    public class Price : INotifyPropertyChanged
    {
        public Price()
        {
            ItemsPrice = new ObservableCollection<ItemPrice>();
            WoodPrice = new ObservableCollection<ItemPrice>();
            BandingPrice = new ObservableCollection<ItemPrice>();
        }

        private ObservableCollection<ItemPrice> _ItemsPrice;
        private ObservableCollection<ItemPrice> _WoodPrice;
        private ObservableCollection<ItemPrice> _BandingPrice;

        public ObservableCollection<ItemPrice> ItemsPrice
        {
            get { return _ItemsPrice; }
            set { _ItemsPrice = value; OnPropertyChanged("ItemsPrice"); }
        }
        public ObservableCollection<ItemPrice> WoodPrice
        {
            get { return _WoodPrice; }
            set { _WoodPrice = value; OnPropertyChanged("WoodPrice"); }
        }
        public ObservableCollection<ItemPrice> BandingPrice
        {
            get { return _BandingPrice; }
            set { _BandingPrice = value; OnPropertyChanged("BandingPrice"); }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public class ItemPrice
    {
        public string Item { get; set; }
        public double Price { get; set; }
        public ItemPrice(string Item, double Price)
        {
            this.Item = Item;
            this.Price = Price;
        }
    }
}
