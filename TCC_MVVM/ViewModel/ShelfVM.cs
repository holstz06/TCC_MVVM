using TCC_MVVM.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using PropertyChanged;

namespace TCC_MVVM.ViewModel
{
    [ImplementPropertyChanged]
    public class ShelfVM : INotifyPropertyChanged
    {
        private DataTable ShelfData { get; set; }
        private DataTable RoomData { get; set; }

        public ObservableCollection<Shelf> Shelves { get; set; }

        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Command to remove shelf from the collection
        /// </summary>
        private ICommand _RemoveCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (_RemoveCommand == null)
                    _RemoveCommand = new CollectionChangeCommand(param => Remove((Shelf)param));
                return _RemoveCommand;
            }
        }

        /// <summary>
        /// Creates a new instance of the shelf view model
        /// </summary>
        /// <param name="ExcelFilePath">
        /// The path to the excel file
        /// </param>
        public ShelfVM(string ExcelFilePath = null)
        {
            ExcelDataTable ExcelDataTable = new ExcelDataTable();
            ShelfData = ExcelDataTable.GetData(ExcelFilePath, "Shelf");
            RoomData = ExcelDataTable.GetData(ExcelFilePath, "Room");

            // Initialize the collections
            Shelves = new ObservableCollection<Shelf>();
        }

        /// <summary>
        /// Gets a list of shelf items
        /// </summary>
        /// <returns>
        /// A list of shelf items
        /// </returns>
        private List<CamPost> GetCamPostItems()
        {
            List<CamPost> camposts = ShelfData.AsEnumerable().Select(row =>
                new CamPost
                {
                    Color = row.Field<string>("Color"),
                    WoodColor = row.Field<string>("WoodColor"),
                    Quantity = (int)row.Field<double>("Quantity"),
                    Price = row.Field<decimal>("Price")
                }).ToList();
            return camposts;
        }

        /// <summary>
        /// Gets the wood color values and prices
        /// </summary>
        /// <returns>
        /// A dictionary of wood colors and prices
        /// </returns>
        private Dictionary<string, decimal> GetWoodValues()
        {
            return RoomData.AsEnumerable()
                .ToDictionary(
                    row => row.Field<string>("WoodColor"),
                    row => row.Field<decimal>("WoodPrice"));
        }

        /// <summary>
        /// Gets the banding color and prices
        /// </summary>
        /// <returns>
        /// A dictionary of banding colors and prices
        /// </returns>
        private Dictionary<string, decimal> GetBandingValues()
        {
            return RoomData.AsEnumerable()
                .ToDictionary(
                    row => row.Field<string>("BandingColor"),
                    row => row.Field<decimal>("BandingPrice"));
        }

        /// <summary>
        /// Removes a shelf from the collection
        /// </summary>
        /// <param name="ShelfModel">
        /// The shelf to remove
        /// </param>
        public void Remove(Shelf ShelfModel)
        {
            if (Shelves.Contains(ShelfModel))
                Shelves.Remove(ShelfModel);
        }

        /// <summary>
        /// Add shelf to the collection
        /// </summary>
        /// <param name="RoomNumber"></param>
        /// <param name="ShelfNumber"></param>
        /// <param name="Color"></param>
        /// <param name="SizeDepth"></param>
        public void Add(int RoomNumber, int ShelfNumber, string Color = null, string SizeDepth = null)
        {
            bool HasColor = false;
            bool HasDepth = false;

            if (Color != null) HasColor = true;
            if (SizeDepth != null) HasDepth = true;

            Shelf shelf = new Shelf(RoomNumber, HasColor ? Color : null, HasDepth ? SizeDepth : null);
            shelf.ColorValues = new ObservableCollection<string>(GetColorValues());
            shelf.WidthValues = new ObservableCollection<string>(GetWidthValues());
            shelf.DepthValues = new ObservableCollection<string>(GetDepthValues());
            shelf.CamPostList = new ObservableCollection<CamPost>(GetCamPostItems());
            shelf.Wood.WoodValues = GetWoodValues();
            shelf.Banding.BandingValues = GetBandingValues();

            shelf.PropertyChanged += Shelf_PropertyChanged;

            Shelves.Add(shelf);
        }

        /// <summary>
        /// Retrieves a list of height values for a panel
        /// </summary>
        /// <returns></returns>
        private List<string> GetColorValues()
        {
            List<string> colorvalues = new List<string>();
            var query = (from row in RoomData.AsEnumerable() select row.Field<string>("WoodColor")).Distinct();
            foreach (string row in query) colorvalues.Add(row);
            return colorvalues;
        }

        /// <summary>
        /// Retrieves a list of height values for a panel
        /// </summary>
        /// <returns></returns>
        private List<string> GetWidthValues()
        {
            List<string> widthvalues = new List<string>();
            var query = (from row in RoomData.AsEnumerable() select row.Field<string>("ShelfWidth")).Distinct();
            foreach (string row in query) widthvalues.Add(row);
            return widthvalues;
        }

        /// <summary>
        /// Retrieves a list of depth values for a panel
        /// </summary>
        /// <returns></returns>
        private List<string> GetDepthValues()
        {
            List<string> depthvalues = new List<string>();
            var query = (from row in RoomData.AsEnumerable() select row.Field<string>("RoomDepth")).Distinct();
            foreach (string row in query) depthvalues.Add(row);
            return depthvalues;
        }

        /// <summary>
        /// Set each shelf color
        /// </summary>
        /// <param name="Color"></param>
        public void SetAllShelfColor(string Color)
        {
            foreach(Shelf shelf in Shelves)
            {
                shelf.Color = Color;
            }
        }

        /// <summary>
        /// Set each shelf depth
        /// </summary>
        /// <param name="SizeDepth">
        /// The depth of the shelf
        /// </param>
        public void SetAllShelfDepth(string SizeDepth)
        {
            foreach (Shelf shelf in Shelves)
            {
                shelf.SizeDepth = SizeDepth;
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void Shelf_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case "Price": SetPriceProperty(sender); break;
                case "Color": SetColorProperty(sender); break;
            }
        }

        void SetPriceProperty(object sender)
        {
            TotalPrice = 0;
            foreach (Shelf shelf in Shelves)
                TotalPrice += shelf.Price;
        }

        void SetColorProperty(object sender)
        {
            Shelf shelf = (Shelf)sender;
        }
        #endregion
    }
}
