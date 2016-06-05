using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data;
using PropertyChanged;
using System;

namespace TCC_MVVM.Model
{
    public enum ShelfType { Adjustable, Fixed }

    [ImplementPropertyChanged]
    public class CamPost : INotifyPropertyChanged
    {
        public int Quantity { get; set; }
        public string Color { get; set; }
        public string WoodColor { get; set; }
        public decimal Price { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    [ImplementPropertyChanged]
    public class Shelf : INotifyPropertyChanged
    {
        private int RoomNumber { get; set; }
        public int ShelfNumber { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
        public string SizeDepth { get; set; }
        public string SizeWidth { get; set; }

        private decimal _Price;
        public decimal Price
        {
           get { return Math.Round(_Price, 2, MidpointRounding.AwayFromZero); }
            set { _Price = value; OnPropertyChanged("Price"); }
        }

        public Wood Wood { get; set; } = new Wood();
        public Banding Banding { get; set; } = new Banding();

        public string ShelfType { get; set; }

        public ObservableCollection<string> ColorValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> WidthValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> DepthValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> ShelfTypeValues { get; set; } = new ObservableCollection<string>() { "Fixed", "Adjustable" };

        public CamPost CamPost { get; set; } = new CamPost();
        public ObservableCollection<CamPost> CamPostList { get; set; } = new ObservableCollection<CamPost>();

        public Shelf(int RoomNumber, string Color = null, string SizeDepth = null)
        {
            Quantity = 1;
            this.RoomNumber = RoomNumber;
            this.Color = Color;
            this.SizeDepth = SizeDepth;
        }

        /// <summary>
        /// Selects the shelf items that belong to this shelf
        /// </summary>
        private void SelectShelfItems()
        {
            if (ShelfType.Equals("Fixed"))
            {
                foreach (CamPost cam in CamPostList)
                {
                    if (cam.WoodColor == Color)
                    {
                        CamPost = cam;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the price of the shelf
        /// </summary>
        private void SetPrice()
        {
            decimal tempPrice = 0;
            tempPrice = 0;

            // Get the price of the wood and banding
            tempPrice += (decimal.Parse(SizeWidth) * decimal.Parse(SizeDepth)) * Wood.Price * (decimal)Wood.MARKUP;
            tempPrice += (decimal.Parse(SizeWidth)) * Banding.Price;
            // Get the price of the campost
            if (ShelfType == "Fixed")
                tempPrice += CamPost.Price * CamPost.Quantity;

            // Get the price for all the boards
            tempPrice += tempPrice * Quantity;
            Price = tempPrice;
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (propertyName == "Color" && !string.IsNullOrEmpty(Color))
            {
                Wood.Color = Color;
                Wood.Price = Wood.WoodValues[Color];
                Banding.Color = Color;
                Banding.Price = Banding.BandingValues[Color];
            }

            if (!string.IsNullOrEmpty(Color)
                && !string.IsNullOrEmpty(SizeWidth)
                && !string.IsNullOrEmpty(SizeDepth)
                && !string.IsNullOrEmpty(ShelfType))
                SelectShelfItems();

            if (!propertyName.Equals("Price")
                && !string.IsNullOrEmpty(Color)
                && !string.IsNullOrEmpty(SizeWidth)
                && !string.IsNullOrEmpty(SizeDepth))
                    SetPrice();
        }
        #endregion
    }
}
