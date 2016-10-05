using System.Windows.Input;
using System.ComponentModel;
using TCC_MVVM.ViewModel.Room.Commands;
using System.Data;
using PropertyChanged;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace TCC_MVVM.ViewModel.Room
{
    [ImplementPropertyChanged]
    public class RoomVM : INotifyPropertyChanged
    {
        // Data Table Variables
        //====================================
        public DataTable Data { get; set; }
        DataTable ShelvingDepthData;
        DataTable StripColorData;
        DataTable WoodData;

        // Cost Variables
        //====================================
        public decimal TotalPrice { get; set; }
        public decimal ShelvingCost { get; set; }
        public decimal StripCost { get; set; }
        public decimal AccessoryCost { get; set; }

        // Model
        public Model.Room Room { get; set; }

        // View Models
        public StripVM StripViewModel { get; set; }
        public PanelVM PanelViewModel { get; set; }
        public ShelfVM ShelfViewModel { get; set; } = new ShelfVM();
        public AccessoryVM AccessoryViewModel { get; set; } = new AccessoryVM();

        public RoomVM()
        {

        }

        /// <summary>
        /// Creates a new instance of a room view model
        /// </summary>
        public RoomVM(string RoomName, int RoomNumber)
        {
            InitializeCommands();
            StripViewModel = new StripVM() { RoomNumber = RoomNumber };
            PanelViewModel = new PanelVM() { RoomNumber = RoomNumber };
            // Subscribe room to view model's properties
            StripViewModel.PropertyChanged += Shelving_PropertyChanged;
            PanelViewModel.PropertyChanged += Shelving_PropertyChanged;
            ShelfViewModel.PropertyChanged += Shelving_PropertyChanged;
            AccessoryViewModel.PropertyChanged += Shelving_PropertyChanged;


            DataSet dataset = new DataSet();
            dataset.ReadXml("StripColorData.xml");
            StripColorData = dataset.Tables[0];

            dataset = new DataSet();
            dataset.ReadXml("ShelvingDepthData.xml");
            ShelvingDepthData = dataset.Tables[0];

            dataset = new DataSet();
            dataset.ReadXml("WoodData.xml");
            WoodData = dataset.Tables[0];


            Room = new Model.Room(RoomName, RoomNumber)
            {
                WoodColorValues = new ObservableCollection<string>(GetWoodColorVales()),
                StripColorValues = new ObservableCollection<string>(GetStripColorValues()),
                ShelvingDepthValues = new ObservableCollection<string>(GetShelvingDepthValues())
            };
            Room.PropertyChanged += Room_PropertyChanged;
        }

        List<string> GetWoodColorVales()
            => WoodData.AsEnumerable().Select(row => row.Field<string>("WoodColor")).Distinct().ToList();

        List<string> GetShelvingDepthValues()
            => ShelvingDepthData.AsEnumerable().Select(row => row.Field<string>("ShelvingDepth")).Distinct().ToList();

        List<string> GetStripColorValues()
            => StripColorData.AsEnumerable().Select(row => row.Field<string>("StripColor")).Distinct().ToList();

        #region Commands

        public void InitializeCommands()
        {
            Command_AddShelf = new Command_AddShelf(this);
            Command_AddAccessory = new Command_AddAccessory(this);
        }

        public ICommand Command_SameStripColor { get; private set; }
        public ICommand Command_AddShelf { get; private set; }
        public ICommand Command_AddWire { get; private set; }
        public ICommand Command_AddAccessory { get; private set; }

        #endregion

        /// <summary>
        /// Set the depth of the panel based on the view's checkbox
        /// </summary>
        bool _IsPanelSameDepth = true;
        public bool IsPanelSameDepth
        {
            get { return _IsPanelSameDepth; }
            set
            {
                _IsPanelSameDepth = value;
                if (value == true)
                    PanelViewModel.SetAllPanelDepth(Room.ShelvingDepth);
                OnPropertyChanged("IsPanelSameDepth");
            }
        }

        /// <summary>
        /// Set the depth of the shelves based on the view's checkbox
        /// </summary>
        bool _IsShelfSameDepth = true;
        public bool IsShelfSameDepth
        {
            get { return _IsShelfSameDepth; }
            set
            {
                _IsShelfSameDepth = value;
                if (value == true)
                    ShelfViewModel.SetAllShelfDepth(Room.ShelvingDepth);
                OnPropertyChanged("IsShelfSameDepth");
            }
        }

        /// <summary>
        /// Same panel color
        /// </summary>
        bool _IsPanelSameColor = true;
        public bool IsPanelSameColor
        {
            get { return _IsPanelSameColor; }
            set
            {
                _IsPanelSameColor = value;
                if (value == true)
                    PanelViewModel.SetAllPanelColor(Room.ShelvingColor);
                OnPropertyChanged("IsPanelSameColor");
            }
        }

        /// <summary>
        /// Same shelf color
        /// </summary>
        bool _IsShelfSameColor = true;
        public bool IsShelfSameColor
        {
            get { return _IsShelfSameColor; }
            set
            {
                _IsShelfSameColor = value;
                if (value == true)
                    ShelfViewModel.SetAllShelfColor(Room.ShelvingColor);
                OnPropertyChanged("IsShelfSameColor");
            }
        }

        /// <summary>
        /// Set strip color
        /// </summary>
        bool _IsStripSameColor = true;
        public bool IsStripSameColor
        {
            get { return _IsStripSameColor; }
            set
            {
                _IsStripSameColor = value;
                if (value == true)
                    StripViewModel.SetAllStripColor(Room.ShelvingColor);
                OnPropertyChanged("IsStripSameColor");
            }
        }

        /// <summary>
        /// Adds shelving to one of the view model's collection
        /// </summary>
        /// <param name="shelvingType">
        /// The type of shelving to be added to
        /// </param>
        public void AddShelvingTo(Model.ShelvingType shelvingType)
        {
            switch(shelvingType)
            {
                case Model.ShelvingType.Shelf:
                    ShelfViewModel.Add(Room.RoomNumber, IsShelfSameColor ? Room.ShelvingColor : null, IsPanelSameDepth ? Room.ShelvingDepth : null);
                    break;

                case Model.ShelvingType.Wire:
                    break;

                case Model.ShelvingType.Accessory:
                    AccessoryViewModel.Add(Room.RoomNumber);
                    break;
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void Shelving_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "TotalPrice")
            {
                TotalPrice = 0;
                TotalPrice +=
                    StripViewModel.TotalPrice +
                    PanelViewModel.TotalPrice +
                    ShelfViewModel.TotalPrice +
                    AccessoryViewModel.TotalPrice;

                // Set the price of the shelving
                ShelvingCost = 0;
                ShelvingCost += PanelViewModel.TotalPrice + ShelfViewModel.TotalPrice;

                // Set the price of the strip
                StripCost = 0;
                StripCost += StripViewModel.TotalPrice;

                // Set the price of the accessory
                AccessoryCost = 0;
                AccessoryCost += AccessoryViewModel.TotalPrice;
            }
        }

        void Room_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case "ShelvingDepth":
                    PanelViewModel.DefaultDepth = (sender as Model.Room).ShelvingDepth;
                    break;
                case "StripColor":
                    StripViewModel.DefaultStripColor = (sender as Model.Room).StripColor;
                    break;
                case "ShelvingColor":
                    PanelViewModel.DefaultColor = (sender as Model.Room).ShelvingColor;
                    break;
            }

        }

        #endregion
    }
}
