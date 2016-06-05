using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Data;
using TCC_MVVM.Model;
using PropertyChanged;
using System;

namespace TCC_MVVM.ViewModel
{
    /// <summary>
    /// Panel View Model
    /// 
    /// Creates a collection of panels that allows the view to display their
    /// properties and create and change them upon request.
    /// </summary>
    [ImplementPropertyChanged]
    public class PanelVM : INotifyPropertyChanged
    {
        private DataTable PanelData;
        private DataTable RoomData;

        /// <summary>
        /// The total price of all the panels
        /// </summary>
        private decimal _TotalPrice;
        public decimal TotalPrice
        {
            get { return Math.Round(_TotalPrice, 2, MidpointRounding.AwayFromZero); }
            set { _TotalPrice = value; OnPropertyChanged("TotalPrice"); }
        }

        /// <summary>
        /// The collection of panels
        /// </summary>
        public ObservableCollection<Panel> Panels { get; set; }
            = new ObservableCollection<Panel>();

        /// <summary>
        /// Command to remove panel from collection
        /// </summary>
        private ICommand _RemoveCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (_RemoveCommand == null)
                    _RemoveCommand = new CollectionChangeCommand(param => Remove((Panel)param));
                return _RemoveCommand;
            }
        }

        /// <summary>
        /// Retrieves a list of panel items
        /// </summary>
        /// <returns></returns>
        private List<PanelItem> GetPanelItems()
        {
            List<PanelItem> panelitems = PanelData.AsEnumerable().Select(row =>
                new PanelItem
                {
                    Name = row.Field<string>("ItemName"),
                    Color = row.Field<string>("Color"),
                    WoodColor = row.Field<string>("WoodColor"),
                    Quantity = row.Field<int>("Quantity"),
                    Price = row.Field<decimal>("Price")
                }).ToList();
            return panelitems;
        }

        /// <summary>
        /// Retrieves a dictionary of wood colors and price
        /// key = color
        /// value = price
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, decimal> GetWoodValues()
        {
            return RoomData.AsEnumerable()
                .ToDictionary(
                    row => row.Field<string>("WoodColor"),
                    row => row.Field<decimal>("WoodPrice"));
        }

        /// <summary>
        /// Retrieves a dictionary of banding colors and price
        /// key = color
        /// value = price
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, decimal> GetBandingValues()
        {
            return RoomData.AsEnumerable()
                .ToDictionary(
                    row => row.Field<string>("BandingColor"),
                    row => row.Field<decimal>("BandingPrice"));
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
        private List<string> GetHeightValues()
        {
            List<string> heightvalues = new List<string>();
            var query = (from row in RoomData.AsEnumerable() select row.Field<string>("PanelHeight")).Distinct();
            foreach (string row in query) heightvalues.Add(row);
            return heightvalues;
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
        /// Create a new instance of an Panel View Model
        /// </summary>
        public PanelVM(string ExcelFilePath)
        {
            ExcelDataTable ExcelDataTable = new ExcelDataTable();
            PanelData = ExcelDataTable.GetData(ExcelFilePath, "Panel");
            RoomData = ExcelDataTable.GetData(ExcelFilePath, "Room");
        }

        /// <summary>
        /// Add a new panel to the collection
        /// </summary>
        /// <param name="RoomNumber"></param>
        /// <param name="PanelNumber"></param>
        /// <param name="Color"></param>
        /// <param name="SizeDepth"></param>
        public void Add(int RoomNumber, int PanelNumber, string Color = null, string SizeDepth = null)
        {
            bool HasColor = false;
            bool HasDepth = false;

            if (Color != null) HasColor = true;
            if (SizeDepth != null) HasDepth = true;

            // Create a new panel
            Panel panel = new Panel(RoomNumber, HasColor ? Color : null, HasDepth ? SizeDepth : null)
            {
                ColorValues = new ObservableCollection<string>(GetColorValues()),
                HeightValues = new ObservableCollection<string>(GetHeightValues()),
                DepthValues = new ObservableCollection<string>(GetDepthValues()),
                PanelItemsList = GetPanelItems()
            };

            panel.Wood.WoodValues = GetWoodValues();
            panel.Banding.BandingValues = GetBandingValues();
            panel.PropertyChanged += Panel_PropertyChanged;

            Panels.Add(panel);
        }

        /// <summary>
        /// Removes a panel from the collection
        /// </summary>
        /// <param name="Panel">
        /// The panel to remove
        /// </param>
        public void Remove(Panel Panel)
        {
            if (Panels.Contains(Panel))
                Panels.Remove(Panel);
        }

        /// <summary>
        /// Sets all the panels in the collection to the same color
        /// </summary>
        /// <param name="Color">
        /// The color to set the panel to
        /// </param>
        public void SetAllPanelColor(string Color)
        {
            foreach (Panel panel in Panels)
            {
                panel.Color = Color;
            }
        }

        /// <summary>
        /// Set all the same panels the same depth
        /// </summary>
        /// <param name="SizeDepth"></param>
        public void SetAllPanelDepth(string SizeDepth)
        {
            foreach (Panel panel in Panels)
            {
                panel.SizeDepth = SizeDepth;
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void Panel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case "Price":
                    TotalPrice = 0;
                    foreach (Panel panel in Panels)
                        TotalPrice += panel.Price;
                    break;
            }
        }
        #endregion
    }
}
