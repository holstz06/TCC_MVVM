using TCC_MVVM.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using PropertyChanged;
using System;
using System.Windows;

namespace TCC_MVVM.ViewModel
{
    [ImplementPropertyChanged]
    public class ShelfVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Data table that stores the shelf items data
        /// </summary>
        private DataTable ShelfData { get; set; }

        /// <summary>
        /// Data table that stores the shelving depth values
        /// </summary>
        private DataTable ShelvingDepthData;

        /// <summary>
        /// Data table that stores the shelf width values
        /// </summary>
        private DataTable ShelfWidthData;

        /// <summary>
        /// Data table that stores the wood data values
        /// </summary>
        private DataTable WoodData;

        /// <summary>
        /// Data table that stores the banding data values
        /// </summary>
        private DataTable BandingData;

        /// <summary>
        /// A collection of shelves
        /// </summary>
        public ObservableCollection<Shelf> Shelves { get; set; } = new ObservableCollection<Shelf>();

        /// <summary>
        /// The total price of all the shelves in the collection
        /// </summary>
        public decimal TotalPrice
        {
            get { return Math.Round(_TotalPrice, 2, MidpointRounding.AwayFromZero); }
            set { _TotalPrice = value; OnPropertyChanged("TotalPrice"); }
        }
        private decimal _TotalPrice;

        /// <summary>
        /// Command to remove a shelf from the collection
        /// </summary>
        public ICommand RemoveCommand
        {
            get
            {
                if (_RemoveCommand == null)
                    _RemoveCommand = new CollectionChangeCommand(param => Remove((Shelf)param));
                return _RemoveCommand;
            }
        }
        private ICommand _RemoveCommand;

        /// <summary>
        /// Creates a new instance of the shelf view model
        /// </summary>
        public ShelfVM()
        {
            /*
             * Attempt to retrieve information from the ShelfData.xml
             * If no information could be retrieved, do nothing but return
             * the error message.
             */
            try
            {
                DataSet dataset = new DataSet();
                dataset.ReadXml("ShelfData.xml");
                ShelfData = dataset.Tables[0];
            }
            catch (Exception e)
            {
                MessageBox.Show("No shelf items could be gathers from the xml.");
                MessageBox.Show(e.ToString());
            }

            /*
             * Attempt to retrieve information from the ShelfWidthData.xml
             * If no information could be retrieved, do nothing and return error message
             */
            try
            {
                DataSet dataset = new DataSet();
                dataset.ReadXml("ShelfWidthData.xml");
                ShelfWidthData = dataset.Tables[0];
            }
            catch (Exception e)
            {
                MessageBox.Show("No shelf width values could be loaded from the xml.");
                MessageBox.Show(e.ToString());
            }

            /*
             * Attempt to retrieve information from the ShelvingDepthData.xml
             * If no information could be retrieved, do nothing and return error message
             */
            try
            {
                DataSet dataset = new DataSet();
                dataset.ReadXml("ShelvingDepthData.xml");
                ShelvingDepthData = dataset.Tables[0];
            }
            catch (Exception e)
            {
                MessageBox.Show("No shelf depth values could be loaded from the xml.");
                MessageBox.Show(e.ToString());
            }

            /*
             * Attempt to retrieve information from the WoodData.xml
             * If no information could be retrieved, do nothing and return error message
             */
            try
            {
                DataSet dataset = new DataSet();
                dataset.ReadXml("WoodData.xml");
                WoodData = dataset.Tables[0];
            }
            catch (Exception e)
            {
                MessageBox.Show("No wood values could be loaded from the xml.");
                MessageBox.Show(e.ToString());
            }

            /*
             * Attempt to retrieve information from the BandingData.xml
             * If no information could be retrieved, do nothing and return error message
             */
            try
            {
                DataSet dataset = new DataSet();
                dataset.ReadXml("BandingData.xml");
                BandingData = dataset.Tables[0];
            }
            catch (Exception e)
            {
                MessageBox.Show("No banding values could be loaded from the xml.");
                MessageBox.Show(e.ToString());
            }
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
                    Quantity = int.Parse(row.Field<string>("Quantity")),
                    Price = decimal.Parse(row.Field<string>("Price"))
                }).ToList();
            return camposts;
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
        /// Retrieves a list of color values
        /// </summary>
        /// <returns>
        /// A list of color values
        /// </returns>
        private List<string> GetColorValues() => WoodData.AsEnumerable().Select(row => row.Field<string>("WoodColor")).Distinct().ToList();

        /// <summary>
        /// Retrieves a list of width values
        /// </summary>
        /// <returns>
        /// A list of width values
        /// </returns>
        private List<string> GetWidthValues() => ShelfWidthData.AsEnumerable().Select(row => row.Field<string>("ShelfWidth")).Distinct().ToList();

        /// <summary>
        /// Retrieves a list of depth values
        /// </summary>
        /// <returns>
        /// A list of depth values
        /// </returns>
        private List<string> GetDepthValues() => ShelvingDepthData.AsEnumerable().Select(row => row.Field<string>("ShelvingDepth")).Distinct().ToList();

        /// <summary>
        /// Gets a dictionary of wood prices by their color
        /// Key = Wood Color
        /// Value = Wood Price
        /// </summary>
        /// <returns>
        /// A dictionary of wood colors and prices
        /// </returns>
        private Dictionary<string, decimal> GetWoodValues()
            => WoodData.AsEnumerable().ToDictionary(row => row.Field<string>("WoodColor"), row => decimal.Parse(row.Field<string>("WoodPrice")));

        /// <summary>
        /// Gets a dictionary of banding prices by their color
        /// Key = Banding Color
        /// Value = Bandin Price
        /// </summary>
        /// <returns>
        /// A dictionary of banding colors and prices
        /// </returns>
        private Dictionary<string, decimal> GetBandingValues()
            => BandingData.AsEnumerable().ToDictionary(row => row.Field<string>("BandingColor"), row => decimal.Parse(row.Field<string>("BandingPrice")));

        /// <summary>
        /// Set each shelf color
        /// </summary>
        /// <param name="Color"></param>
        public void SetAllShelfColor(string Color)
        {
            foreach(Shelf shelf in Shelves)
                shelf.Color = Color;
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
                shelf.SizeDepth = SizeDepth;
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
