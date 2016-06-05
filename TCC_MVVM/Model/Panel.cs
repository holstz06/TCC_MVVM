using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using PropertyChanged;
using System;

namespace TCC_MVVM.Model
{
    /// <summary>
    /// Type of drilled panel
    /// </summary>
    public enum PanelDrill
    {
        Right,  // Stop Drilled - Rght Hand
        Left,   // Stop Drilled - Left Hand
        Thru    // Thru Drilled
    }

    /// <summary>
    /// Accessory items that belong to the panel
    /// </summary>
    [ImplementPropertyChanged]
    public class PanelItem : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public string WoodColor { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    [ImplementPropertyChanged]
    public class Panel : INotifyPropertyChanged
    {
        public int Quantity { get; set; }
        public int RoomNumber { get; set; }
        public int PanelNumber { get; set; }
        public string SizeHeight { get; set; }
        public string SizeDepth { get; set; }
        public string Color { get; set; } = null;

        private decimal _Price;
        public decimal Price
        {
            get { return Math.Round(_Price, 2, MidpointRounding.AwayFromZero); }
            set { _Price = value; OnPropertyChanged("Price"); }
        }

        public Wood Wood { get; set; }
        public Banding Banding { get; set; }

        public ObservableCollection<string> ColorValues { get; set; }
        public ObservableCollection<string> HeightValues { get; set; }
        public ObservableCollection<string> DepthValues { get; set; }
        public ObservableCollection<PanelItem> PanelItems { get; set; }
        public List<PanelItem> PanelItemsList { get; set; }

        public Panel(int RoomNumber, string Color = null, string SizeDepth = null)
        {
            // Initialize the collections
            ColorValues = new ObservableCollection<string>();
            HeightValues = new ObservableCollection<string>();
            DepthValues = new ObservableCollection<string>();
            PanelItems = new ObservableCollection<PanelItem>();
            PanelItemsList = new List<PanelItem>();

            Wood = new Wood();
            Banding = new Banding();

            Quantity = 1; // Default the quantity to 1
            this.RoomNumber = RoomNumber;
            this.Color = Color;
            this.SizeDepth = SizeDepth;
        }

        /// <summary>
        /// Creates new instnace of Panel
        /// </summary>
        public Panel()
        {

        }

        /// <summary>
        /// Selects the subitems that belong to this panel
        /// </summary>
        private void SelectPanelItems()
        {
            PanelItems.Clear();
            foreach(PanelItem item in PanelItemsList)
            {
                if (item.WoodColor == Color || item.Color == "N/A")
                    PanelItems.Add(item);
            }
        }

        /// <summary>
        /// Sets the price of this panel by adding the price of the subitems
        /// and adding the cost of wood and banding
        /// </summary>
        private decimal SetPrice()
        {
            decimal price = 0;
            decimal fees = (decimal)(Wood.PANEL_INSTALL + Wood.ROUTER_FEE + Wood.DRILLBIT_FEE);

            // Calculate the cost of wood and banding
            decimal WoodPrice = (decimal.Parse(SizeHeight) * decimal.Parse(SizeDepth)) * Wood.Price;
            decimal BandingPrice = (decimal.Parse(SizeHeight) + (2 * decimal.Parse(SizeDepth))) * Banding.Price;

            price += ((WoodPrice + BandingPrice) * (decimal)Wood.MARKUP) + fees;

            // Add on the additional prices from the panel items
            foreach (PanelItem item in PanelItems)
                price += item.Price;

            price *= Quantity;
            return price;
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
            if (!string.IsNullOrEmpty(Color) && !string.IsNullOrEmpty(SizeHeight) && !string.IsNullOrEmpty(SizeDepth))
                SelectPanelItems();
            if (propertyName != "Price" && !string.IsNullOrEmpty(Color) && !string.IsNullOrEmpty(SizeHeight) && !string.IsNullOrEmpty(SizeDepth))
                Price = SetPrice();
        }
        #endregion
    }
}
