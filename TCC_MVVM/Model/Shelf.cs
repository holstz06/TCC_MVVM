using System.ComponentModel;
using System.Collections.ObjectModel;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TCC_MVVM.Model
{ 
    [ImplementPropertyChanged]
    public class Shelf : INotifyPropertyChanged
    {
        public int RoomNumber { get; set; }
        public int ShelfNumber { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
        public string SizeDepth { get; set; }
        public string SizeWidth { get; set; }


        public decimal LaborFees { get; set; } = 2M;
        public decimal EquipmentFees { get; set; } = 0.5M;



        public string ShelfTypeName { get; set; }

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

        public Wood Wood { get; set; } = new Wood();
        public Banding Banding { get; set; } = new Banding();
        public ShelfType ShelfType { get; set; } = new ShelfType();

        public ObservableCollection<string> ColorValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> WidthValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> DepthValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> ShelfTypeValues { get; set; } = new ObservableCollection<string>();

        ///=================================================================================================
        /// <summary>
        ///     Creates new instance of a shelf - Default
        /// </summary>
        ///=================================================================================================
        public Shelf()
        {

        }

        ///=================================================================================================
        /// <summary>
        ///     Constructor - Creates a new instance of a shelf
        /// </summary>
        /// 
        /// <param name="Color">
        ///     The color of this shelf
        /// </param>
        /// 
        /// <param name="RoomNumber">
        ///     (Optional) The room number this shelf belongs to
        /// </param>
        /// 
        /// <param name="SizeDepth">
        ///     (Optional) The depth of this shelf
        /// </param>
        ///=================================================================================================
        public Shelf(int RoomNumber, string Color = null, string SizeDepth = null)
        {
            Quantity = 1;
            this.RoomNumber = RoomNumber;
            this.Color = Color;
            this.SizeDepth = SizeDepth;
        }

        ///=================================================================================================
        /// <summary>
        ///     Sets the price of the shelf
        /// </summary>
        /// 
        /// <remarks>
        ///     <para>Wood Cost = Width * Depth * Wood Price * Markup Price</para>
        ///     <para>Banding Cost = Width * Banding Price</para>
        ///     <para>Campost Cost = Price per cam * quantity (by type e.g. shoe shelf = 2 cams)</para>
        ///     <para>TOTAL COST = (Wood + Banding + Campost + Fence + Top Connector + Toe Kick) * Quantity</para>
        /// </remarks>
        ///=================================================================================================
        void SetPrice()
        {
            decimal tempPrice = 0;

            var width = decimal.Parse(this.SizeWidth);
            var depth = decimal.Parse(this.SizeDepth);

            // Get the price of the wood and banding
            tempPrice += (width * depth) * Wood.Price;
            tempPrice += (width) * Banding.Price;

            tempPrice += EquipmentFees;
            tempPrice += LaborFees;

            tempPrice += (ShelfType.CamPostQuantity * ShelfType.CamPostPrice);

            if (ShelfType.HasFence)
                tempPrice += ShelfType.FencePrice;
            if (ShelfType.HasTopConnector)
                tempPrice += ShelfType.TopConnectorPrice;
            if(ShelfType.IsToeKick)
                tempPrice += width * 3 /*HEIGHT*/ * Wood.Price;

            tempPrice *= (decimal)Wood.MARKUP;

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
    }
}
