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
        /// The room number all the strip in the collection will have
        /// </summary>
        public int RoomNumber { get; set; }
        /// <summary>
        /// The default strip color for this room
        /// </summary>
        public string DefaultStripColor { get; set; }
        /// <summary>
        /// The datatable that hold the strip items
        /// </summary>
        DataTable StripData;
        /// <summary>
        /// The datatable that hold the strip color values
        /// </summary>
        DataTable StripColorData;
        /// <summary>
        /// A collection of strip
        /// </summary>
        public ObservableCollection<Strip> Strips { get; set; } = new ObservableCollection<Strip>();
        /// <summary>
        /// A list of all the strip colors
        /// </summary>
        List<string> stripColors = new List<string>();
        /// <summary>
        /// A list of all the strip items
        /// </summary>
        List<StripItem> stripItems = new List<StripItem>();
        /// <summary>
        /// The price of all the strip in the collection
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// Command to duplicate strip
        /// </summary>
        public ICommand DuplicateStripCommand { get; private set; }
        /// <summary>
        /// Command to remove a strip from the collection
        /// </summary>
        public ICommand RemoveCommand { get; private set; }
        /// <summary>
        /// Command to add a strip to the collection
        /// </summary>
        public ICommand AddCommand { get; private set; }

        /// <summary>
        /// Create new instance of strip view model
        /// </summary>
        public StripVM()
        {
            Initialize();

            // Create commands
            DuplicateStripCommand = new StripCommands.AddCommand(this);
            RemoveCommand = new StripCommands.RemoveCommand(this);
            AddCommand = new StripCommands.AddCommand(this);
        }

        /// <summary>
        /// Initialize the strip view model by setting up datatables and initializing lists
        /// </summary>
        void Initialize()
        {
            try
            {
                StripData = CreateDataTable("StripData.xml");
                StripColorData = CreateDataTable("StripColorData.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            // Initialize all the strip colors by querying the StripColor datatable
            stripColors = StripColorData.AsEnumerable().Select(row =>
                row.Field<string>("StripColor")).Distinct().ToList();

            // Initialize all the strip items by querying the StripData datatable
            stripItems = StripData.AsEnumerable().Select(row => new StripItem
            {
                Name = row.Field<string>("ItemName"),
                Color = row.Field<string>("Color"),
                Quantity = int.Parse(row.Field<string>("Quantity")),
                Price = decimal.Parse(row.Field<string>("Price"))
            }).ToList();
        }

        /// <summary>
        /// Creates a data table from an XML file
        /// </summary>
        /// <param name="path">The path of the XML fle</param>
        /// <returns>The datatable read from the file</returns>
        DataTable CreateDataTable(string path)
        {
            DataSet dataset = new DataSet();
            dataset.ReadXml(path);
            return dataset.Tables[0];
        }

        /// <summary>
        /// Sets the properties of a new strip then adds it to the collection
        /// </summary>
        public void AddStrip()
        {
            var strip = new Strip(RoomNumber, DefaultStripColor)
            {
                StripItems = GetListofStripItems(DefaultStripColor),
                ColorValues = new ObservableCollection<string>(stripColors),
                viewmodel = this,

            };
            strip.PropertyChanged += Strip_PropertyChanged;
            Strips.Add(strip);
        }

        /// <summary>
        /// Adds a strip to the collection
        /// </summary>
        /// <param name="strip"></param>
        public void AddStrip(Strip strip)
        {
            strip.StripItems = GetListofStripItems(DefaultStripColor);
            strip.ColorValues = new ObservableCollection<string>(stripColors);
            strip.viewmodel = this;
            strip.PropertyChanged += Strip_PropertyChanged;
            Strips.Add(strip);
        }

        /// <summary>
        /// Removes a strip from the collection
        /// </summary>
        /// <param name="Strip">
        /// The strip to remove from the collection
        /// </param>
        public void Remove(Strip Strip)
        {
            if (Strip != null)
            {
                // Remove the price before you remove the strip, otherwise the price will stay the same
                TotalPrice -= Strip.Price;
                if (Strips.Contains(Strip))
                    Strips.Remove(Strip);
            }
            else
                MessageBox.Show("You must select an item before removing it.");
            
        }

        /// <summary>
        /// Sets each of the strip in the collection to the same color
        /// </summary>
        /// <param name="Color">The color to set the strip to</param>
        public void SetAllStripColor(string Color)
        {
            foreach(var strip in Strips)
                strip.Color = Color;
        }
        
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void Strip_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                // Strip's price changed
                case "Price":
                    TotalPrice = 0;
                    foreach (Strip strip in Strips)
                        TotalPrice += strip.Price;
                    break;

                // Strip's color changed
                case "Color":
                    (sender as Strip).StripItems = GetListofStripItems((sender as Strip).Color);
                    break;
            }
        }

        List<StripItem> GetListofStripItems(string stripColor)
        {
            List<StripItem> newStripItems = new List<StripItem>();
            foreach (var item in stripItems)
            {
                if (item.Color == stripColor || item.Color == "N/A")
                    newStripItems.Add(item);
            }
            return newStripItems;
        }
        #endregion
    }
}