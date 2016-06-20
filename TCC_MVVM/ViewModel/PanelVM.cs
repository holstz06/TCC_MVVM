using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Data;
using TCC_MVVM.Model;
using PropertyChanged;
using System;
using System.Windows;

namespace TCC_MVVM.ViewModel
{
    [ImplementPropertyChanged]
    public class PanelVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Data table of all the panel data (items)
        /// </summary>
        private DataTable PanelData;

        /// <summary>
        /// Data table of height values for panels
        /// </summary>
        private DataTable PanelHeightData;

        /// <summary>
        /// Data table of depth values for panels
        /// </summary>
        private DataTable ShelvingDepthData;

        /// <summary>
        /// Data table of wood values and prices
        /// </summary>
        private DataTable WoodData;

        /// <summary>
        /// Data table of banding values and prices
        /// </summary>
        private DataTable BandingData;

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
        public ObservableCollection<Panel> Panels { get; set; } = new ObservableCollection<Panel>();

        /// <summary>
        /// A list of panel items
        /// </summary>
        private List<PanelItem> PanelItems { get; set; } = new List<PanelItem>();

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
        /// Retrieves a list of panel items that belong to the panel.
        /// </summary>
        /// <returns>
        /// A list of panel items
        /// </returns>
        private List<PanelItem> GetPanelItems()
        {
            return PanelData.AsEnumerable().Select(row =>
                new PanelItem
                {
                    Name = row.Field<string>("ItemName"),
                    Color = row.Field<string>("Color"),
                    WoodColor = row.Field<string>("WoodColor"),
                    Quantity = int.Parse(row.Field<string>("Quantity")),
                    Price = decimal.Parse(row.Field<string>("Price"))
                }).ToList();
        }

        /// <summary>
        /// Retrieves a dictionary of wood colors and price
        /// key = color
        /// value = price
        /// </summary>
        /// <returns>
        /// Dictionary of wood values
        /// </returns>
        private Dictionary<string, decimal> GetWoodValues()
        {
            return WoodData.AsEnumerable()
                .ToDictionary(
                    row => row.Field<string>("WoodColor"),
                    row => decimal.Parse(row.Field<string>("WoodPrice")));
        }

        /// <summary>
        /// Retrieves a dictionary of banding colors and price
        /// key = color
        /// value = price
        /// </summary>
        /// <returns>
        /// Dictionary of banding values
        /// </returns>
        private Dictionary<string, decimal> GetBandingValues()
        {
            return BandingData.AsEnumerable()
                .ToDictionary(
                    row => row.Field<string>("BandingColor"),
                    row => decimal.Parse(row.Field<string>("BandingPrice")));
        }

        /// <summary>
        /// Retrieves a list of wood color values.
        /// These values are generated from the WoodData.xml
        /// </summary>
        /// <returns>
        /// List of wood color values
        /// </returns>
        private List<string> GetColorValues() => WoodData.AsEnumerable().Select(row => row.Field<string>("WoodColor")).Distinct().ToList();

        /// <summary>
        /// Retrieves a list of height values. 
        /// These values are generated from the PanelHeightData.xml
        /// </summary>
        /// <returns>
        /// List of height values
        /// </returns>
        private List<string> GetHeightValues() => PanelHeightData.AsEnumerable().Select(row => row.Field<string>("PanelHeight")).Distinct().ToList();


        /// <summary>
        /// Retrieves a list of depth values.
        /// These values are generated from the ShelvingDepthData.xml
        /// </summary>
        /// <returns>
        /// List of depth values
        /// </returns>
        private List<string> GetDepthValues() => ShelvingDepthData.AsEnumerable().Select(row => row.Field<string>("ShelvingDepth")).Distinct().ToList();

        /// <summary>
        /// Create a new instance of an Panel View Model
        /// </summary>
        public PanelVM()
        {
            try
            {
                DataSet dataset = new DataSet();
                dataset.ReadXml("PanelData.xml");
                PanelData = dataset.Tables[0];
            }
            catch (Exception e)
            {
                MessageBox.Show("No panel items could be gathers from the xml.");
                MessageBox.Show(e.ToString());
            }

            try
            {
                DataSet dataset = new DataSet();
                dataset.ReadXml("PanelHeightData.xml");
                PanelHeightData = dataset.Tables[0];
            }
            catch (Exception e)
            {
                MessageBox.Show("No panel height values could be loaded from the xml.");
                MessageBox.Show(e.ToString());
            }

            try
            {
                DataSet dataset = new DataSet();
                dataset.ReadXml("ShelvingDepthData.xml");
                ShelvingDepthData = dataset.Tables[0];
            }
            catch (Exception e)
            {
                MessageBox.Show("No panel depth values could be loaded from the xml.");
                MessageBox.Show(e.ToString());
            }

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

            // Initialize the panel items
            PanelItems = GetPanelItems();
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
                panel.Color = Color;
        }

        /// <summary>
        /// Set all the same panels the same depth
        /// </summary>
        /// <param name="SizeDepth"></param>
        public void SetAllPanelDepth(string SizeDepth)
        {
            foreach (Panel panel in Panels)
                panel.SizeDepth = SizeDepth;
        }

        #region INotifyPropertyChanged Members
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
