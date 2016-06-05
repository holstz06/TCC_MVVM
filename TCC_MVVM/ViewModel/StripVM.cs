using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Input;
using PropertyChanged;

using TCC_MVVM.Model;
using TCC_MVVM.Data;

namespace TCC_MVVM.ViewModel
{
    [ImplementPropertyChanged]
    public class StripVM : INotifyPropertyChanged
    {
        private DataTable StripData;
        private DataTable RoomData;

        public ObservableCollection<Strip> Strips { get; set; }
            = new ObservableCollection<Strip>();

        public decimal TotalPrice { get; set; }

        public TCCData Data { get; set; }

        /// <summary>
        /// Command to remove strip from the collection
        /// </summary>
        private ICommand _RemoveCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (_RemoveCommand == null)
                    _RemoveCommand = new CollectionChangeCommand(param => Remove((Strip)param));
                return _RemoveCommand;
            }
        }

        /// <summary>
        /// Create new instance of strip view model
        /// </summary>
        /// <param name="ExcelFilePath">
        /// The path to the Excel file to pull data from
        /// </param>
        public StripVM(string ExcelFilePath = null)
        {
            ExcelDataTable ExcelDataTable = new ExcelDataTable();
            StripData = ExcelDataTable.GetData(ExcelFilePath, "Strip");
            RoomData = ExcelDataTable.GetData(ExcelFilePath, "Room");
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
                    Quantity = row.Field<int>("Quantity"),
                    Price = row.Field<decimal>("Price")
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
            List<string> colorvalues = new List<string>();
            var query = (from row in RoomData.AsEnumerable() select row.Field<string>("StripColor")).Distinct();

            foreach(string row in query)
                colorvalues.Add(row);

            return colorvalues;
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
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
