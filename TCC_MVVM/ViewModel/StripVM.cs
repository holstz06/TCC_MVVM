using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Input;
using PropertyChanged;

using TCC_MVVM.Model;
using System;
using System.Windows;

namespace TCC_MVVM.ViewModel
{
    [ImplementPropertyChanged]
    public class StripVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Data table that contains the items that belong to strip shelving
        /// </summary>
        private DataTable StripData;

        /// <summary>
        /// Data table that contains colors of the strip
        /// </summary>
        private DataTable StripColorData;

        /// <summary>
        /// A collection of strip
        /// </summary>
        public ObservableCollection<Strip> Strips { get; set; }
            = new ObservableCollection<Strip>();

        /// <summary>
        /// The total price of the collection of strip
        /// </summary>
        public decimal TotalPrice
        {
            get { return Math.Round(_TotalPrice, 2, MidpointRounding.AwayFromZero); }
            set { _TotalPrice = value; OnPropertyChanged("TotalPrice"); }
        }
        private decimal _TotalPrice;

        /// <summary>
        /// Command to remove strip from the collection
        /// </summary>
        public ICommand RemoveCommand
        {
            get
            {
                if (_RemoveCommand == null)
                    _RemoveCommand = new CollectionChangeCommand(param => Remove((Strip)param));
                return _RemoveCommand;
            }
        }
        private ICommand _RemoveCommand;

        /// <summary>
        /// Create new instance of strip view model
        /// </summary>
        public StripVM()
        {
            /*
             * Attempt to retrieve information from the StripData.xml
             * If no information could be retrieved, do nothing but return
             * the error message.
             */
            try
            {
                DataSet dataset = new DataSet();
                dataset.ReadXml("StripData.xml");
                StripData = dataset.Tables[0];
            }
            catch(Exception e)
            {
                MessageBox.Show("No strip items could be gathers from the xml.");
                MessageBox.Show(e.ToString());
            }
            
            /*
             * Attempt to retrieve information from the StripColorData.xml
             * 
             * If no information could be retrieved, load a set of default color
             * values and return the error message.
             */
            try
            {
                DataSet dataset = new DataSet();
                dataset.ReadXml("StripColorData.xml");
                StripColorData = dataset.Tables[0];
            }
            catch(Exception e)
            {
                MessageBox.Show("No strip colors could be loaded from the xml.");
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// Gets a list of strip items
        /// </summary>
        /// <returns>
        /// A list of strip items
        /// </returns>
        public List<StripItem> GetStripItems()
        {
            return StripData.AsEnumerable().Select(row =>
                new StripItem
                {
                    Name = row.Field<string>("ItemName"),
                    Color = row.Field<string>("Color"),
                    Quantity = int.Parse(row.Field<string>("Quantity")),
                    Price = decimal.Parse(row.Field<string>("Price"))
                }).ToList();
        }

        /// <summary>
        /// Creates a list of color values from the data table
        /// </summary>
        /// <returns>
        /// Return a list of color values
        /// </returns>
        private List<string> GetStripColors()
        {
            return StripColorData.AsEnumerable().Select(row => row.Field<string>("StripColor")).Distinct().ToList();
        }

        /// <summary>
        /// Sets the properties of a new strip then adds it to the collection
        /// </summary>
        /// <param name="RoomNumber">
        /// The room number this strip belongs to
        /// </param>
        /// <param name="StripNumber">
        /// A unique ID for this strip
        /// </param>
        /// <param name="Color">
        /// The color of this strip
        /// </param>
        public void Add(int RoomNumber, int StripNumber, string Color = null)
        {
            bool HasColor = false;
            if(Color != null) HasColor = true;

            Strip strip = new Strip(RoomNumber,  HasColor ? Color : null);
            strip.StripItemList = GetStripItems();
            strip.ColorValues = new ObservableCollection<string>(GetStripColors());

            // Subscribe to property change events that happen in the strip model
            strip.PropertyChanged += Strip_PropertyChanged;

            // Add strip to the collection
            Strips.Add(strip);
        }

        /// <summary>
        /// Adds a strip to the collection
        /// </summary>
        /// <param name="strip">
        /// The strip to add to the collection
        /// </param>
        public void Add(Strip strip)
        {
            strip.StripItemList = GetStripItems();
            strip.ColorValues = new ObservableCollection<string>(GetStripColors());
            strip.PropertyChanged += Strip_PropertyChanged;
            Strips.Add(strip);
        }

        /// <summary>
        /// Removes a strip from the collection
        /// </summary>
        /// <param name="StripModel">
        /// The strip to remove from the collection
        /// </param>
        public void Remove(Strip StripModel)
        {
            if (Strips.Contains(StripModel))
                Strips.Remove(StripModel);
        }

        /// <summary>
        /// Sets each strip in the collection to the same color
        /// </summary>
        /// <param name="Color">
        /// The color to set the strip to
        /// </param>
        public void SetAllStripColor(string Color)
        {
            foreach(Strip strip in Strips)
                strip.Color = Color;
        }
        
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies the observers of change, xalled by the Set accessor of a property to 
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property
        /// </param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Creates a new listener so the observer of this object can see this property
        /// </summary>
        void Strip_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Price")
            {
                TotalPrice = 0;
                foreach (Strip strip in Strips)
                    TotalPrice += strip.Price;
            }
        }
        #endregion
    }
}