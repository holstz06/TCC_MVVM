using System.Windows.Input;
using System.ComponentModel;
using TCC_MVVM.ViewModel.Room.Commands;
using System.Data;
using PropertyChanged;

namespace TCC_MVVM.ViewModel.Room
{
    /// <summary>
    /// The type of shelvig units
    /// </summary>
    public enum ShelvingType
    {
        Panel,
        Shelf,
        Wire,
        Strip,
        Accessory
    }

    [ImplementPropertyChanged]
    public class RoomVM : INotifyPropertyChanged
    {
        public string ExcelFilePath { get; set; }
        public DataTable Data { get; set; }
        public decimal TotalPrice { get; set; }

        // Model
        public Model.Room Room { get; set; }

        // View Models
        public StripVM StripViewModel { get; set; }
        public PanelVM PanelViewModel { get; set; }
        public ShelfVM ShelfViewModel { get; set; }
        public AccessoryVM AccessoryViewModel { get; set; }

        /// <summary>
        /// Creates a new instance of a room view model
        /// </summary>
        /// <param name="ExcelFilePath">
        /// The path to the Excel file
        /// </param>
        /// <param name="RoomName">
        /// The name of this room
        /// </param>
        /// <param name="RoomNumber">
        /// The number of this room
        /// </param>
        public RoomVM(string ExcelFilePath, string RoomName, int RoomNumber = 0)
        {
            InitializeCommands();
            this.ExcelFilePath = ExcelFilePath;

            // Initialize the view models
            StripViewModel = new StripVM(ExcelFilePath);
            PanelViewModel = new PanelVM(ExcelFilePath);
            ShelfViewModel = new ShelfVM(ExcelFilePath);
            AccessoryViewModel = new AccessoryVM(ExcelFilePath);

            // Subscribe room to view model's properties
            StripViewModel.PropertyChanged += Shelving_PropertyChanged;
            PanelViewModel.PropertyChanged += Shelving_PropertyChanged;
            ShelfViewModel.PropertyChanged += Shelving_PropertyChanged;
            AccessoryViewModel.PropertyChanged += Shelving_PropertyChanged;

            // Set the Room Model
            Model.ExcelDataTable ExcelDataTable = new Model.ExcelDataTable();
            Data = ExcelDataTable.GetData(ExcelFilePath, "Room");
            Room = new Model.Room(Data, RoomName, RoomNumber);
        }

        #region Commands

        public void InitializeCommands()
        {
            Command_AddStrip = new Command_AddStrip(this);
            Command_AddPanel = new Command_AddPanel(this);
            Command_AddShelf = new Command_AddShelf(this);
            Command_AddAccessory = new Command_AddAccessory(this);
        }

        public ICommand Command_SameStripColor { get; private set; }

        // Add Commands
        public ICommand Command_AddStrip { get; private set; }
        public ICommand Command_AddPanel { get; private set; }
        public ICommand Command_AddShelf { get; private set; }
        public ICommand Command_AddWire { get; private set; }
        public ICommand Command_AddAccessory { get; private set; }

        #endregion

        /// <summary>
        /// Set the depth of the panel based on the view's checkbox
        /// </summary>
        private bool _IsPanelSameDepth = true;
        public bool IsPanelSameDepth
        {
            get { return _IsPanelSameDepth; }
            set
            {
                _IsPanelSameDepth = value;
                if (value == true)
                    PanelViewModel.SetAllPanelDepth(Room.RoomDepth);
                OnPropertyChanged("IsPanelSameDepth");
            }
        }

        /// <summary>
        /// Set the depth of the shelves based on the view's checkbox
        /// </summary>
        private bool _IsShelfSameDepth = true;
        public bool IsShelfSameDepth
        {
            get { return _IsShelfSameDepth; }
            set
            {
                _IsShelfSameDepth = value;
                if (value == true)
                    ShelfViewModel.SetAllShelfDepth(Room.RoomDepth);
                OnPropertyChanged("IsShelfSameDepth");
            }
        }

        /// <summary>
        /// Same panel color
        /// </summary>
        private bool _IsPanelSameColor = true;
        public bool IsPanelSameColor
        {
            get { return _IsPanelSameColor; }
            set
            {
                _IsPanelSameColor = value;
                if (value == true)
                    PanelViewModel.SetAllPanelColor(Room.RoomColor);
                OnPropertyChanged("IsPanelSameColor");
            }
        }

        /// <summary>
        /// Same shelf color
        /// </summary>
        private bool _IsShelfSameColor = true;
        public bool IsShelfSameColor
        {
            get { return _IsShelfSameColor; }
            set
            {
                _IsShelfSameColor = value;
                if (value == true)
                    ShelfViewModel.SetAllShelfColor(Room.RoomColor);
                OnPropertyChanged("IsShelfSameColor");
            }
        }

        /// <summary>
        /// Set strip color
        /// </summary>
        private bool _IsStripSameColor = true;
        public bool IsStripSameColor
        {
            get { return _IsStripSameColor; }
            set
            {
                _IsStripSameColor = value;
                if (value == true)
                    StripViewModel.SetAllStripColor(Room.RoomColor);
                OnPropertyChanged("IsStripSameColor");
            }
        }

        private int PanelIndex, ShelfIndex, StripIndex = 0;

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
                case Model.ShelvingType.Panel:
                     PanelViewModel.Add(Room.RoomNumber, PanelIndex++, IsPanelSameColor ? Room.RoomColor : null, IsPanelSameDepth ? Room.RoomDepth : null);
                     break;

                case Model.ShelvingType.Strip:
                    StripViewModel.Add(Room.RoomNumber, StripIndex++, IsStripSameColor ? Room.StripColor : null);
                    break;

                case Model.ShelvingType.Shelf:
                    ShelfViewModel.Add(Room.RoomNumber, ShelfIndex++, IsShelfSameColor ? Room.RoomColor : null, IsPanelSameDepth ? Room.RoomDepth : null);
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
                    ShelfViewModel.TotalPrice;
                    //AccessoryViewModel.TotalPrice;
            }
        }
        #endregion
    }
}
