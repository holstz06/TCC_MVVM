using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using PropertyChanged;
using System.Collections.Generic;

namespace TCC_MVVM.Model
{
    /// <summary>
    /// Strip Item (Model)
    /// 
    /// Sub-items that each strip contains
    /// </summary>
    [ImplementPropertyChanged]
    public class StripItem : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Strip (Model)
    /// 
    /// Contains all the business data memebrs that belong to a strip and functions
    /// that set the property members
    /// </summary>
    [ImplementPropertyChanged]
    public class Strip : INotifyPropertyChanged
    {
        public int RoomNumber { get; set; }
        public int StripNumber { get; set; }
        public double Length { get; set; }
        public string Color { get; set; }

        private decimal _Price;
        public decimal Price
        {
            // Return the price to two decimal places
            get { return Math.Round(_Price, 2, MidpointRounding.AwayFromZero); }
            set { _Price = value; OnPropertyChanged("Price"); }
        }

        public ObservableCollection<string> ColorValues { get; set; }
            = new ObservableCollection<string>();
        public ObservableCollection<StripItem> StripItems { get; set; }
            = new ObservableCollection<StripItem>();
        public List<StripItem> StripItemList { get; set; }
            = new List<StripItem>();

        /// <summary>
        /// Creates a new instance of a strip
        /// </summary>
        /// <param name="RoomNumber">
        /// The room number this strip belongs to
        /// </param>
        /// <param name="Color">
        /// The color this strip belongs to
        /// </param>
        public Strip(int RoomNumber, string Color = null)
        {
            this.RoomNumber = RoomNumber;
            this.Color = Color;
        }

        /// <summary>
        /// Creates a new instnace of a strip
        /// </summary>
        public Strip()
        {

        }

        /// <summary>
        /// Iterates through each strip item and selects which item belongs to this collection
        /// </summary>
        private void SelectStripItems()
        {
            StripItems.Clear();
            foreach (StripItem item in StripItemList)
            {
                if (item.Color == Color || item.Color == "N/A")
                    StripItems.Add(item);
            }
        }
        
        /// <summary>
        /// Sets the price of the strip by adding all the sub item prices
        /// 
        /// * Note: Price is rounded to two deciaml places
        /// </summary>
        private void SetPrice()
        {
            decimal tempPrice = 0;
            foreach (StripItem item in StripItems)
                tempPrice += item.Price * (decimal)Length;
            Price = tempPrice;
        }

        /// <summary>
        /// Sets the value of a property by its property name
        /// </summary>
        /// <param name="PropertyName">
        /// The name of the property
        /// </param>
        /// <param name="PropertyValue">
        /// The value of the property
        /// </param>
        public void SetProperty(string PropertyName, string PropertyValue)
        {
            switch (PropertyName)
            {
                case "RoomNumber": RoomNumber = int.Parse(PropertyValue); break;
                case "Length": Length = double.Parse(PropertyValue); break;
                case "Color": Color = PropertyValue; break;
                case "Price": Price = decimal.Parse(PropertyValue); break;
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (!string.IsNullOrEmpty(Color) && Length != 0)
                SelectStripItems();
            if (propertyName != "Price" && !string.IsNullOrEmpty(Color) && Length != 0)
                SetPrice();
        }
        #endregion
    }
}
