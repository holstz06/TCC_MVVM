using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using PropertyChanged;
using System.Collections.Generic;

namespace TCC_MVVM.Model
{
    [ImplementPropertyChanged]
    public class StripItem : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    [ImplementPropertyChanged]
    public class Strip : INotifyPropertyChanged
    {
        public int RoomNumber { get; set; }
        public string Color { get; set; }
        public double Length { get; set; }
        

        decimal _Price;
        public decimal Price
        {
            // Return the price to two decimal places
            get { return Math.Round(_Price, 2, MidpointRounding.AwayFromZero); }
            set { _Price = value; OnPropertyChanged("Price"); }
        }

        public ObservableCollection<string> ColorValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<StripItem> StripItems { get; set; } = new ObservableCollection<StripItem>();
        public List<StripItem> StripItemList { get; set; } = new List<StripItem>();

        /// <summary>
        /// Creates a new instance of a strip
        /// </summary>
        public Strip()
        {

        }

        /// <summary>
        /// Creates a new instance of a strip
        /// </summary>
        /// <param name="RoomNumber">
        /// The room number this strip belongs to
        /// </param>
        /// <param name="Color">
        /// The color of the strip
        /// </param>
        public Strip(int RoomNumber, string Color = null)
        {
            this.RoomNumber = RoomNumber;
            this.Color = Color;
        }

        /// <summary>
        /// Iterates through each strip item and selects which item belongs to this collection
        /// </summary>
        void SelectStripItems()
        {
            StripItems.Clear();
            foreach (StripItem item in StripItemList)
            {
                if (item.Color == Color || item.Color == "N/A")
                    StripItems.Add(item);
            }
        }

        /// <summary>
        /// Sets the price of the strip by adding all the subitem prices.
        /// These include the wall rail and cover strip
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

            if (!string.IsNullOrEmpty(Color) && Length != 0)
                SelectStripItems();
            if (propertyName != "Price" && !string.IsNullOrEmpty(Color) && Length != 0)
                SetPrice();
        }
        #endregion

        public readonly string[] Properties =
{
            "RoomNumber",
            "Color",
            "Length",
            "Price"
        };

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

        string _DisplayName;
        public string DisplayName
        {
            get
            {
                return "1x (" + Color + ") Hang Rail/Cover, " + Length + "in. ";
            }
            set { _DisplayName = value; OnPropertyChanged("DisplayName"); }
        }
    }
}
