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
        /// A collection of panels
        /// </summary>
        public ObservableCollection<Panel> Panels { get; set; } = new ObservableCollection<Panel>();
        /// <summary>
        /// The default pane color
        /// </summary>
        public string DefaultColor { get; set; }
        /// <summary>
        /// The default panel dept
        /// </summary>
        public string DefaultDepth { get; set; }
        /// <summary>
        /// The default room number
        /// </summary>
        public int RoomNumber { get; set; }
        /// <summary>
        /// The data containing items for the panel
        /// </summary>
        DataTable PanelData;
        /// <summary>
        /// Data for the height values
        /// </summary>
        DataTable PanelHeightData;
        /// <summary>
        /// Data for the shelving depths
        /// </summary>
        DataTable ShelvingDepthData;
        /// <summary>
        /// Data for the wood
        /// </summary>
        DataTable WoodData;
        /// <summary>
        /// Data for the banding
        /// </summary>
        DataTable BandingData;
        /// <summary>
        /// A list of panel items
        /// </summary>
        List<PanelItem> PanelItems { get; set; } = new List<PanelItem>();
        /// <summary>
        /// Wood values by color and price
        /// </summary>
        Dictionary<string, decimal> Woodvalues;
        /// <summary>
        /// Banding values by color and price
        /// </summary>
        Dictionary<string, decimal> BandingValues;
        /// <summary>
        /// The total quantity of all the panel
        /// </summary>
        public int TotalQuantity { get; set; }
        /// <summary>
        /// The total price of all the panels
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// The panel selected in the view
        /// </summary>
        public Panel SelectedPanel { get; set; }
        /// <summary>
        /// The panel index selected in the view
        /// </summary>
        public int SelectedPanelIndex { get; set; }
        /// <summary>
        /// A list of panels selected in the view
        /// </summary>
        public List<Panel> SelectedPanels { get; set; }

        public ICommand SelectNextCommand { get; private set; }
        public ICommand SelectPreviousCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand IncrementQuantityCommand { get; private set; }
        public ICommand DecrementQuantityCommand { get; private set; }

        /// <summary>
        /// Constructor - Creates new instance of panel view model
        /// </summary>
        public PanelVM()
        {
            SelectNextCommand = new PanelCommands.SelectNextCommand(this);
            SelectPreviousCommand = new PanelCommands.SelectPreviousCommand(this);
            RemoveCommand = new PanelCommands.RemoveCommand(this);
            AddCommand = new PanelCommands.AddCommand(this);
            IncrementQuantityCommand = new PanelCommands.IncrementQuantityCommand(this);
            DecrementQuantityCommand = new PanelCommands.DecrementQuantityCommand(this);
            bool isValid = false;
            try
            {
                PanelData = GetDataTable("PanelData.xml");
                PanelHeightData = GetDataTable("PanelHeightData.xml");
                ShelvingDepthData = GetDataTable("ShelvingDepthData.xml");
                WoodData = GetDataTable("WoodData.xml");
                BandingData = GetDataTable("BandingData.xml");
                isValid = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            if(isValid)
            {
                PanelItems = GetPanelItems();
                Woodvalues = GetWoodValues();
                BandingValues = GetBandingValues();
            }
        }

        /// <summary>
        /// Reads the value from xml files to datatables
        /// </summary>
        /// <param name="path">The path to the xml file</param>
        /// <returns>The datatable read from the xml file</returns>
        DataTable GetDataTable(string path)
        {
            DataSet dataset = new DataSet();
            dataset.ReadXml(path);
            return dataset.Tables[0];
        }

        /// <summary>
        /// Gets a list of items that belong to the panel
        /// </summary>
        /// <returns></returns>
        List<PanelItem> GetPanelItems()
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
        /// Retrieves the values of wood
        /// </summary>
        /// <returns>A dictionary of wood colors and price</returns>
        Dictionary<string, decimal> GetWoodValues()
            => WoodData.AsEnumerable()
                .ToDictionary(
                    row => row.Field<string>("WoodColor"),
                    row => decimal.Parse(row.Field<string>("WoodPrice")));

        /// <summary>
        /// Retrieves the values of banding
        /// </summary>
        /// <returns>A dictionary of banding color and prices</returns>
        Dictionary<string, decimal> GetBandingValues()
            => BandingData.AsEnumerable()
                .ToDictionary(
                    row => row.Field<string>("BandingColor"),
                    row => decimal.Parse(row.Field<string>("BandingPrice")));

        /// <summary>
        /// Gets a list of wood colors
        /// </summary>
        /// <returns></returns>
        List<string> GetColorValues() => WoodData.AsEnumerable().Select(row => row.Field<string>("WoodColor")).Distinct().ToList();

        /// <summary>
        /// Gets a list of height values
        /// </summary>
        /// <returns></returns>
        List<string> GetHeightValues() => PanelHeightData.AsEnumerable().Select(row => row.Field<string>("PanelHeight")).Distinct().ToList();


        /// <summary>
        /// Gets a list of depth values
        /// </summary>
        /// <returns></returns>
        List<string> GetDepthValues() => ShelvingDepthData.AsEnumerable().Select(row => row.Field<string>("ShelvingDepth")).Distinct().ToList();

        /// <summary>
        /// Adds a new panel to the collection
        /// </summary>
        /// <param name="RoomNumber">The panel's room number</param>
        /// <param name="Color">The color of the panel</param>
        /// <param name="SizeDepth">The depth of the panel</param>
        public void Add(int RoomNumber, string Color = null, string SizeDepth = null)
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
                viewmodel = this,
                PanelItemsList = GetPanelItems()
            };

            TotalQuantity += 1;

            panel.PropertyChanged += Panel_PropertyChanged;
            if(Color != null)
                Color_PropertyChange(panel);
            Panels.Add(panel);
        }

        /// <summary>
        /// Adds a new panel
        /// </summary>
        /// <param name="panel">The panel to add</param>
        public void Add(Panel panel)
        {
            panel.ColorValues = new ObservableCollection<string>(GetColorValues());
            panel.HeightValues = new ObservableCollection<string>(GetHeightValues());
            panel.DepthValues = new ObservableCollection<string>(GetDepthValues());
            panel.PanelItemsList = GetPanelItems();
            panel.viewmodel = this;
            TotalQuantity += panel.Quantity;
            panel.PropertyChanged += Panel_PropertyChanged;
            Panels.Add(panel);
        }

        ///**********************************************************************************
        /// <summary>
        ///     Removes a panel from the collection
        /// </summary>
        /// 
        /// <param name="panel">
        ///     The panel to remove
        /// </param>
        ///**********************************************************************************
        public void Remove(Panel panel)
        {
            if (Panels.Contains(panel))
            {
                TotalPrice -= panel.Price;
                TotalQuantity -= panel.Quantity;
                Panels.Remove(panel);
            }
        }

        ///**********************************************************************************
        /// <summary>
        ///     Sets all the panels in the collection to the same color
        /// </summary>
        /// 
        /// <param name="Color">
        ///     The color to set the panels to
        /// </param>
        ///**********************************************************************************
        public void SetAllPanelColor(string Color)
        {
            foreach (Panel panel in Panels)
                panel.Color = Color;
        }

        ///**********************************************************************************
        /// <summary>
        ///     Sets all the panels in the collection to the same depth
        /// </summary>
        /// 
        /// <param name="SizeDepth">
        ///     The depth to set the panels to
        /// </param>
        ///**********************************************************************************
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
        void OnPropertyChanged(string propertyName)
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
                case "Color":
                    Color_PropertyChange(sender as Panel);
                    break;
                case "Quantity":
                    TotalQuantity = 0;
                    foreach (Panel panel in Panels)
                        TotalQuantity += panel.Quantity;
                    break;
            }
        }

        void Color_PropertyChange(Panel panel)
        {
            // Set the value of the wood (color and price)
            panel.Wood.Color = panel.Color;
            panel.Wood.Price = Woodvalues[panel.Color];

            // Set the value of the banding (color and price)
            panel.Banding.Color = panel.Color;
            panel.Banding.Price = BandingValues[panel.Color];
        }
        #endregion
    }
}
