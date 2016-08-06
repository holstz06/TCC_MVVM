using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using PropertyChanged;
using System.Collections.Generic;
using System.Data;

namespace TCC_MVVM.Model
{
    /// <summary>
    /// Accessory items attached to a strip
    /// </summary>
    public class StripItem
    {
        /// <summary>
        /// The name of the strip item
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The color of the strip item (if there is one)
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// The quantity of the strip item
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// The price of the strip item
        /// </summary>
        public decimal Price { get; set; }
    }

    /// <summary>
    /// The rail that holds the panels
    /// </summary>
    [ImplementPropertyChanged]
    public class Strip : INotifyPropertyChanged, ICloneable
    {
        /// <summary>
        /// The room number this strip belongs to
        /// </summary>
        public int RoomNumber { get; set; }
        /// <summary>
        /// The color of the strip
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// The length of the strip
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// The price of the strip
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// A collection of color values to choose from
        /// </summary>
        public ObservableCollection<string> ColorValues { get; set; } = new ObservableCollection<string>();
        /// <summary>
        /// A list of items that also belong to this strip
        /// </summary>
        public List<StripItem> StripItems { get; set; } = new List<StripItem>();
        /// <summary>
        /// The viewmodel that contains this strip
        /// </summary>
        public ViewModel.StripVM viewmodel { get; set; }

        /// <summary>
        /// Default - Creates a new instance of a strip
        /// </summary>
        public Strip()
        {

        }

        /// <summary>
        /// Creates a new instance of this trip
        /// </summary>
        /// <param name="RoomNumber">The room number the strip belongs to</param>
        /// <param name="Color">(Optional) The color of tthe strip</param>
        public Strip(int RoomNumber, string Color = null)
        {
            this.RoomNumber = RoomNumber;
            this.Color = Color;
        }

        /// <summary>
        /// Sets the price of the strip including the price of all its items
        /// </summary>
        void SetPrice()
        {
            decimal tempPrice = 0;
            foreach (StripItem item in StripItems)
                tempPrice += item.Price * (decimal)Length;
            Price = tempPrice;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (propertyName != "Price" && !string.IsNullOrEmpty(Color) && Length != 0)
                SetPrice();
        }
        #endregion

        /// <summary>
        /// The properties of this strip
        /// </summary>
        public readonly string[] Properties =
        {
            "RoomNumber",
            "Color",
            "Length",
            "Price"
        };

        /// <summary>
        /// Sets a property based on the property's name
        /// </summary>
        /// <param name="PropertyName">The property's name</param>
        /// <param name="PropertyValue">The property's value</param>
        public void SetProperty(string PropertyName, string PropertyValue)
        {
            switch (PropertyName)
            {
                case "RoomNumber": RoomNumber = int.Parse(PropertyValue); break;
                case "Color": Color = PropertyValue; break;
                case "Length": Length = double.Parse(PropertyValue); break;
                case "Price": Price = decimal.Parse(PropertyValue); break;
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// The name to display to the view
        /// </summary>
        public string DisplayName
        {
            get
            {
                return "1x (" + Color + ") Hang Rail/Cover, " + Length + "\"";
            }
        }
    }
}
