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
    [ImplementPropertyChanged]
    public class PanelVM : INotifyPropertyChanged
    {
        // Data Tables Vairables
        //================================
        DataTable PanelData;
        DataTable PanelHeightData;
        DataTable ShelvingDepthData;
        DataTable WoodData;
        DataTable BandingData;

        // Collections Variables
        //=================================
        public ObservableCollection<Panel> Panels { get; set; } = new ObservableCollection<Panel>();
        List<PanelItem> PanelItems { get; set; } = new List<PanelItem>();
        Dictionary<string, decimal> Woodvalues;
        Dictionary<string, decimal> BandingValues;

        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public double TotalSqIn { get; set; }

        // Commands
        //=================================
        ICommand _RemoveCommand;
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
        /// Create a new instance of an Panel View Model
        /// </summary>
        public PanelVM()
        {
            DataSet dataset = new DataSet();
            dataset.ReadXml("PanelData.xml");
            PanelData = dataset.Tables[0];

            dataset = new DataSet();
            dataset.ReadXml("PanelHeightData.xml");
            PanelHeightData = dataset.Tables[0];

            dataset = new DataSet();
            dataset.ReadXml("ShelvingDepthData.xml");
            ShelvingDepthData = dataset.Tables[0];

            dataset = new DataSet();
            dataset.ReadXml("WoodData.xml");
            WoodData = dataset.Tables[0];

            dataset = new DataSet();
            dataset.ReadXml("BandingData.xml");
            BandingData = dataset.Tables[0];

            // Initialize the panel items
            PanelItems = GetPanelItems();
            Woodvalues = GetWoodValues();
            BandingValues = GetBandingValues();
        }

        /// <summary>
        /// Retrieves a list of panel items that belong to the panel.
        /// </summary>
        /// <returns>
        /// A list of panel items
        /// </returns>
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
        /// Retrieves a dictionary of wood colors and price
        /// key = color
        /// value = price
        /// </summary>
        /// <returns>
        /// Dictionary of wood values
        /// </returns>
        Dictionary<string, decimal> GetWoodValues()
            => WoodData.AsEnumerable()
                .ToDictionary(
                    row => row.Field<string>("WoodColor"),
                    row => decimal.Parse(row.Field<string>("WoodPrice")));

        /// <summary>
        /// Retrieves a dictionary of banding colors and price
        /// key = color
        /// value = price
        /// </summary>
        /// <returns>
        /// Dictionary of banding values
        /// </returns>
        Dictionary<string, decimal> GetBandingValues()
            => BandingData.AsEnumerable()
                .ToDictionary(
                    row => row.Field<string>("BandingColor"),
                    row => decimal.Parse(row.Field<string>("BandingPrice")));

        /// <summary>
        ///     Retrieves a list of wood color values.
        ///     These values are generated from the WoodData.xml
        /// </summary>
        /// 
        /// <returns>
        ///     List of wood color values
        /// </returns>
        List<string> GetColorValues() => WoodData.AsEnumerable().Select(row => row.Field<string>("WoodColor")).Distinct().ToList();

        /// <summary>
        ///     Retrieves a list of height values. 
        ///     These values are generated from the PanelHeightData.xml
        /// </summary>
        /// 
        /// <returns>
        ///     List of height values
        /// </returns>
        List<string> GetHeightValues() => PanelHeightData.AsEnumerable().Select(row => row.Field<string>("PanelHeight")).Distinct().ToList();


        /// <summary>
        /// Retrieves a list of depth values.
        /// These values are generated from the ShelvingDepthData.xml
        /// </summary>
        /// <returns>
        /// List of depth values
        /// </returns>
        List<string> GetDepthValues() => ShelvingDepthData.AsEnumerable().Select(row => row.Field<string>("ShelvingDepth")).Distinct().ToList();

        

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

            TotalQuantity += 1;

            panel.PropertyChanged += Panel_PropertyChanged;
            if(Color != null)
                Color_PropertyChange(panel);
            Panels.Add(panel);
        }

        public void Add(Panel panel)
        {
            panel.ColorValues = new ObservableCollection<string>(GetColorValues());
            panel.HeightValues = new ObservableCollection<string>(GetHeightValues());
            panel.DepthValues = new ObservableCollection<string>(GetDepthValues());
            panel.PanelItemsList = GetPanelItems();
            TotalQuantity += panel.Quantity;
            TotalSqIn += (double.Parse(panel.SizeDepth) * double.Parse(panel.SizeHeight));
            panel.PropertyChanged += Panel_PropertyChanged;
            Panels.Add(panel);
        }

        /// <summary>
        /// Removes a panel from the collection
        /// </summary>
        /// <param name="panel">
        /// The panel to remove
        /// </param>
        public void Remove(Panel panel)
        {
            TotalPrice -= panel.Price;
            TotalQuantity -= panel.Quantity;
            TotalSqIn -= (double.Parse(panel.SizeHeight) * double.Parse(panel.SizeDepth));
            if (Panels.Contains(panel))
                Panels.Remove(panel);
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
                case "SizeDepth":
                    TotalSqIn = 0;
                    foreach (Panel panel in Panels)
                        TotalSqIn += (double.Parse(panel.SizeDepth) * double.Parse(panel.SizeHeight));
                    break;
                case "SizeHeight":
                    TotalSqIn = 0;
                    foreach (Panel panel in Panels)
                        TotalSqIn += (double.Parse(panel.SizeDepth) * double.Parse(panel.SizeHeight));
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
