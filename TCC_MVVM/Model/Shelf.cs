using System.ComponentModel;
using System.Collections.ObjectModel;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TCC_MVVM.Model
{
    /// <summary>
    /// A closet shelf
    /// </summary>
    [ImplementPropertyChanged]
    public class Shelf : INotifyPropertyChanged, ICloneable
    {
        public ViewModel.ShelfVM viewmodel { get; set; }

        /// <summary>
        /// The room number this shelf belongs to
        /// </summary>
        public int RoomNumber { get; set; }
        /// <summary>
        /// The quantity of this shelf
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// The color of this shelf
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// The depth of this shelf
        /// </summary>
        public string SizeDepth { get; set; }
        /// <summary>
        /// The width of this shelf
        /// </summary>
        public string SizeWidth { get; set; }
        /// <summary>
        /// The labor fees associated with making this shelf
        /// </summary>
        public decimal LaborFees { get; set; } = 2M;
        /// <summary>
        /// The equipment fees associated with making this shelf
        /// </summary>
        public decimal EquipmentFees { get; set; } = 0.5M;
        /// <summary>
        /// The name of this shelf's type
        /// </summary>
        public string ShelfTypeName { get; set; }
        /// <summary>
        /// The price of this shelf
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// The wood this shelf is made of
        /// </summary>
        public Wood Wood { get; set; } = new Wood();
        /// <summary>
        /// The banding this shelf is made of
        /// </summary>
        public Banding Banding { get; set; } = new Banding();
        /// <summary>
        /// The type of shelf this is (Shoe shelf, corner shelf, adjustable, etc.)
        /// </summary>
        public ShelfType ShelfType { get; set; } = new ShelfType();
        /// <summary>
        /// A collection of color values
        /// </summary>
        public ObservableCollection<string> ColorValues { get; set; } = new ObservableCollection<string>();
        /// <summary>
        /// A collection of width values
        /// </summary>
        public ObservableCollection<string> WidthValues { get; set; } = new ObservableCollection<string>();
        /// <summary>
        /// A collection of depth values
        /// </summary>
        public ObservableCollection<string> DepthValues { get; set; } = new ObservableCollection<string>();
        /// <summary>
        /// A collection of shelf type values
        /// </summary>
        public ObservableCollection<string> ShelfTypeValues { get; set; } = new ObservableCollection<string>();

        /// <summary>
        /// Constructor - Creates new instance of a shelf model
        /// </summary>
        public Shelf()
        {

        }

        /// <summary>
        /// Constructor - Creates new instance of a shelf model
        /// </summary>
        /// <param name="RoomNumber">This shelf's room number</param>
        /// <param name="Color">This shelf's color</param>
        /// <param name="SizeDepth">The depth of this shelf</param>
        public Shelf(int RoomNumber, string Color = null, string SizeDepth = null)
        {
            Quantity = 1;
            this.RoomNumber = RoomNumber;
            this.Color = Color;
            this.SizeDepth = SizeDepth;
        }

        /// <summary>
        /// Sets the price of this shelf
        /// </summary>
        void SetPrice()
        {
            decimal tempPrice = 0;

            var width = decimal.Parse(SizeWidth);
            var depth = decimal.Parse(SizeDepth);

            var woodPrice = (width * depth) * Wood.Price;
            var bandingPrice = (width) * Banding.Price;

            tempPrice += woodPrice + bandingPrice + EquipmentFees + LaborFees;

            tempPrice += (ShelfType.CamPostQuantity * ShelfType.CamPostPrice);

            if (ShelfType.HasFence)
                tempPrice += ShelfType.FencePrice;

            if (ShelfType.HasTopConnector)
                tempPrice += ShelfType.TopConnectorPrice;

            if (ShelfType.IsToeKick)
                tempPrice += width * 4 /*HEIGHT*/ * Wood.Price;

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

        /// <summary>
        /// Closes this model
        /// </summary>
        /// <returns></returns>
        public object Clone() => MemberwiseClone();

        /// <summary>
        /// The name displayed on the view
        /// </summary>
        public string DisplayName
        {
            get { return Quantity + "x " + ShelfType.Name + " Shelf, " + Color + ", " + SizeWidth + "in. x " + SizeDepth + "in. "; }
        }

        /// <summary>
        /// A list of all this shelves properties
        /// </summary>
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

        /// <summary>
        /// Sets a property based on property name
        /// </summary>
        /// <param name="PropertyName">The name of the property</param>
        /// <param name="PropertyValue">The value of the property</param>
        public void SetProperty(string PropertyName, string PropertyValue)
        {
            switch (PropertyName)
            {
                case "RoomNumber": RoomNumber = int.Parse(PropertyValue); break;
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
    }
}
