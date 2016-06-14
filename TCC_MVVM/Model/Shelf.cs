using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data;
using PropertyChanged;
using System;

namespace TCC_MVVM.Model
{
    /// <summary>
    /// Camposts are items fastened to a shelf to lock them into panels.
    /// Only fixed shelfs have these items but depending on the shelf,
    /// they vary how many they have (e.g. Shoe shelves have 2 where corner 
    /// shelves have 6)
    /// 
    /// Camposts also have a wood color associated with it. In other words,
    /// every wood color has its own campost color
    /// </summary>
    public class CamPost
    {
        public int Quantity { get; set; }
        public string Color { get; set; }
        public string WoodColor { get; set; }
        public decimal Price { get; set; }
    }

    /// <summary>
    /// Fences are only used when the shelf is slanted, which prevents
    /// the contents of the shelf from sliding off. Typically, only used
    /// for shoe shelves
    /// </summary>
    public class Fence
    {
        public bool HasFence { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
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
        public CamPost CamPost { get; set; } = new CamPost();
        public Fence Fence { get; set; } = new Fence();

        /// <summary>
        /// The shelf type (Fixed, Adjustable, etc.)
        /// </summary>
        public string ShelfType { get; set; }

        /// <summary>
        /// A collection of color values
        /// </summary>
        public ObservableCollection<string> ColorValues { get; set; } = new ObservableCollection<string>();

        /// <summary>
        /// A collection of widt values
        /// </summary>
        public ObservableCollection<string> WidthValues { get; set; } = new ObservableCollection<string>();

        /// <summary>
        /// A collection of depth values
        /// </summary>
        public ObservableCollection<string> DepthValues { get; set; } = new ObservableCollection<string>();

        /// <summary>
        /// A collection of the names of shelf types
        /// </summary>
        public ObservableCollection<string> ShelfTypeValues { get; set; } = new ObservableCollection<string>();

        /// <summary>
        /// Creates a new instance of a shelf
        /// </summary>
        /// <param name="RoomNumber">
        /// The unique room number this shelf belongs to
        /// </param>
        /// <param name="Color">
        /// The color of this shelf
        /// </param>
        /// <param name="SizeDepth">
        /// The depth of this shelf
        /// </param>
        public Shelf(int RoomNumber, string Color = null, string SizeDepth = null)
        {
            Quantity = 1;
            this.RoomNumber = RoomNumber;
            this.Color = Color;
            this.SizeDepth = SizeDepth;
        }

        /// <summary>
        /// Sets the campost and fence information
        /// </summary>
        private void SetShelfItem()
        {
            string tempShelfType = ShelfType;
            if (tempShelfType.Contains("Shoe Shelf"))
                tempShelfType = "Shoe";

            switch (tempShelfType)
            {
                case "Adjustable":      CamPost.Quantity = 0; Fence.HasFence = false; break;
                case "Fixed":           CamPost.Quantity = 4; Fence.HasFence = false; break;
                case "(Adj) Corner":    CamPost.Quantity = 0; Fence.HasFence = false; break;
                case "(Fixed) Corner":  CamPost.Quantity = 6; Fence.HasFence = false; break;
                case "Shoe":            CamPost.Quantity = 2; Fence.HasFence = true; break;
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
            tempPrice += (CamPost.Price * CamPost.Quantity);

            // Get the price of the fence
            if (Fence.HasFence)
                tempPrice += Fence.Price;

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
                SetShelfItem();

            if (!propertyName.Equals("Price")
                && !string.IsNullOrEmpty(Color)
                && !string.IsNullOrEmpty(SizeWidth)
                && !string.IsNullOrEmpty(SizeDepth)
                && !string.IsNullOrEmpty(ShelfType))
                    SetPrice();
        }
        #endregion

        #region Display Name

        private string _DisplayName;
        public string DisplayName
        {
            get
            {
                string tempShelfType = ShelfType;
                if (tempShelfType.Contains("Shoe Shelf"))
                    tempShelfType = "Shoe";

                switch(tempShelfType)
                {
                    case "Adjustable":  return Quantity + "x (" + Color + ") Adj. Shelf: " + SizeDepth + "in. x " + SizeWidth + "in. ";
                    case "Fixed":       return Quantity + "x (" + Color + ") Fixed Shelf: " + SizeDepth + "in. x " + SizeWidth + "in. ";
                    case "(Adj) Corner":   return Quantity + "x (" + Color + ") Adj. Corner Shelf: " + SizeDepth + "in. x " + SizeWidth + "in. ";
                    case "(Fixed) Corner": return Quantity + "x (" + Color + ") Fixed Corner Shelf: " + SizeDepth + "in. x " + SizeWidth + "in. ";
                    case "Shoe":        return Quantity + "x (" + Color + ") Shoe Shelf w/ " + Fence.Color + " Fence : " + SizeDepth + "in. x " + SizeWidth + "in. ";
                }
                return "Shelf";

            }
            private set
            {
                _DisplayName = value; OnPropertyChanged("DisplayName");
            }
        }
        #endregion
    }
}
