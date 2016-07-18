using System.ComponentModel;
using System.Collections.ObjectModel;
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

    /// <summary>
    /// An object that describes the type of shelf
    /// </summary>
    public class ShelfType
    {
        public string Name { get; set; }
        public int CamQuantity { get; set; }
        public bool HasFencePost { get; set; }
        public string FencePostColor { get; set; } = "";
    }

    [ImplementPropertyChanged]
    public class Shelf : INotifyPropertyChanged
    {
        //========================================
        // Shelving variables
        //========================================
        public int RoomNumber { get; set; }
        public int ShelfNumber { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
        public string SizeDepth { get; set; }
        public string SizeWidth { get; set; }

        //========================================
        // Cam Post Variables
        //========================================
        public string CamPostColor { get; set; }
        public int CamPostQuantity { get; set; }
        public decimal CamPostPrice { get; set; }

        //========================================
        // Fence Post Variables
        //========================================
        public bool HasFence { get; set; }
        public string FenceColor { get; set; }
        public decimal FencePrice { get; set; }

        //========================================
        // Shelf Price
        //========================================
        private decimal _Price;
        public decimal Price
        {
            get { return Math.Round(_Price, 2, MidpointRounding.AwayFromZero); }
            set { _Price = value; OnPropertyChanged("Price"); }
        }

        //========================================
        // Dependancy Variables
        //========================================
        public Wood Wood { get; set; } = new Wood();
        public Banding Banding { get; set; } = new Banding();
        public ShelfType ShelfType { get; set; } = new ShelfType();

        //========================================
        // Collection Variables
        //========================================
        public ObservableCollection<string> ColorValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> WidthValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> DepthValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<ShelfType> ShelfTypeValues { get; set; } = new ObservableCollection<ShelfType>();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Shelf()
        {

        }

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
        /// Sets the price of the shelf
        /// </summary>
        private void SetPrice()
        {
            Price = 0; // Reset price

            decimal SizeWidth = decimal.Parse(this.SizeWidth);
            decimal SizeDepth = decimal.Parse(this.SizeDepth);

            // Get the price of the wood and banding
            Price += (SizeWidth * SizeDepth) * Wood.Price * (decimal)Wood.MARKUP;
            Price += (SizeWidth) * Banding.Price;

            Price += (CamPostQuantity * CamPostPrice);

            if (HasFence)
                Price += FencePrice;
            
            // Get the price for all the boards
            Price += Price * Quantity;
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (!propertyName.Equals("Price")
                && !string.IsNullOrEmpty(Color)
                && !string.IsNullOrEmpty(SizeWidth)
                && !string.IsNullOrEmpty(SizeDepth)
                && !string.IsNullOrEmpty(ShelfType.Name))
                    SetPrice();
        }
        #endregion

        #region Display Name

        private string _DisplayName;
        public string DisplayName
        {
            get
            {
                return Quantity + "x " + ShelfType.Name + ", " + Color + ", " + SizeWidth + "in. x " + SizeDepth + "in. ";
            }
            private set
            {
                _DisplayName = value; OnPropertyChanged("DisplayName");
            }
        }
        #endregion
    }
}
