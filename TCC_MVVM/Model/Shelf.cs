using System.ComponentModel;
using System.Collections.ObjectModel;
using PropertyChanged;
using System;

namespace TCC_MVVM.Model
{
    /// <summary>
    /// Camposts are items fastened to a shelf to lock them into panels.
    /// Only fixed shelfs have these items but depending on the shelf,
    /// they vary how many they have (e.g. Shoe shelves have 2, corner 
    /// shelves have 6)
    /// 
    /// Camposts also have a wood color associated with it. Every wood color has its own campost color
    /// </summary>
    public class CamPost
    {
        public int Quantity { get; set; }
        public string Color { get; set; }
        public string WoodColor { get; set; }
        public decimal Price { get; set; }
    }

    /// <summary>
    /// Top Connector (H beams) are attached to the side of a shelf to secure it to another
    /// shelf adjacent. This useful for corners.
    /// </summary>
    public class TopConnector
    {
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
        // Top Connector Variables
        //========================================
        public bool HasTopConnector { get; set; }
        public string TopConnectorColor { get; set; }
        public decimal TopConnectorPrice { get; set; }

        //========================================
        // Toe Kick Variables
        //========================================
        public bool IsToeKick { get; set; }
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
        public decimal LaborFees { get; set; } 
        public decimal EquipmentFees { get; set; } 
        public string ShelfTypeName { get; set; }

        //========================================
        // Shelf Price
        //========================================
        decimal _Price;
        public decimal Price
        {
            get { return Math.Round(_Price, 2, MidpointRounding.AwayFromZero); }
            set { _Price = value; OnPropertyChanged("Price"); }
        }

        public readonly string[] Properties =
        {
            "RoomNumber",
            "ShelfNumber",
            "Quantity",
            "Color",
            "SizeDepth",
            "SizeWidth",
            "LaborFees",
            "EquipmentFees",
            "Price",
            "ShelfTypeName"
        };

        public void SetProperty(string PropertyName, string PropertyValue)
        {
            switch(PropertyName)
            {
                case "RoomNumber": RoomNumber = int.Parse(PropertyValue); break;
                case "ShelfNumber": ShelfNumber = int.Parse(PropertyValue); break;
                case "Quantity": Quantity = int.Parse(PropertyValue); break;
                case "Color": Color = PropertyValue; break;
                case "SizeDepth": SizeDepth = PropertyValue; break;
                case "SizeWidth": SizeWidth = PropertyValue; break;
                case "LaborFees": LaborFees = decimal.Parse(PropertyValue); break;
                case "EquipmentFees": EquipmentFees = decimal.Parse(PropertyValue); break;
                case "Price": Price = decimal.Parse(PropertyValue); break;
                case "ShelfTypeName": ShelfTypeName = PropertyValue; break;
            }
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
        public ObservableCollection<string> ShelfTypeValues { get; set; } = new ObservableCollection<string>();

        /// <summary>
        /// Default Constructor - Creates new instance of a shelf
        /// </summary>
        public Shelf()
        {

        }

        /// <summary>
        /// Creates a new instance of a shelf
        /// </summary>
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
        void SetPrice()
        {
            decimal tempPrice = 0; // Reset price

            var SizeWidth = decimal.Parse(this.SizeWidth);
            var SizeDepth = decimal.Parse(this.SizeDepth);

            // Get the price of the wood and banding
            tempPrice += (SizeWidth * SizeDepth) * Wood.Price * (decimal)Wood.MARKUP;
            tempPrice += (SizeWidth) * Banding.Price;

            tempPrice += (ShelfType.CamPostQuantity * ShelfType.CamPostPrice);

            if (ShelfType.HasFence)
                tempPrice += ShelfType.FencePrice;
            if (ShelfType.HasTopConnector)
                tempPrice += ShelfType.TopConnectorPrice;
            if(ShelfType.IsToeKick)
                tempPrice += SizeWidth * 3 /*HEIGHT*/ * Wood.Price;


            // Get the price for all the boards
            tempPrice *= Quantity;

            Price = tempPrice;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (!propertyName.Equals("Price")
                && !string.IsNullOrEmpty(Color)
                && !string.IsNullOrEmpty(SizeWidth)
                && !string.IsNullOrEmpty(SizeDepth)
                && !string.IsNullOrEmpty(ShelfTypeName))
                    SetPrice();
        }

        string _DisplayName;
        public string DisplayName
        {
            get
            {
                return Quantity + "x " + ShelfType.Name + " Shelf, " + Color + ", " + SizeWidth + "in. x " + SizeDepth + "in. ";
            }
            private set
            {
                _DisplayName = value; OnPropertyChanged("DisplayName");
            }
        }

        public ShelfType ShelfType1
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }
    }
}
