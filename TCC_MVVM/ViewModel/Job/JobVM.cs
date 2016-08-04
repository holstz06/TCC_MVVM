using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TCC_MVVM.ViewModel.Room;
using PropertyChanged;
using TCC_MVVM.Model;
using System.Windows;

namespace TCC_MVVM.ViewModel.Job
{
    [ImplementPropertyChanged]
    public class JobVM : INotifyPropertyChanged
    {
        /// <summary>
        /// The current room number, increments everytime a room is created
        /// </summary>
        int CurrentRoomNumber { get; set; } = 0;
        /// <summary>
        /// The total price of all the rooms
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// The cost of shelving alone
        /// </summary>
        public decimal ShelvingCost { get; set; }
        /// <summary>
        /// The cost of strip alone
        /// </summary>
        public decimal StripCost { get; set; }
        /// <summary>
        /// The cost of accessories alone
        /// </summary>
        public decimal AccessoryCost { get; set; }

        /// <summary>
        /// <para>The state that the display is in (Data Entry View - Proposal View)</para>
        /// <para>0 = Split view (Data Entry / Proposal View)</para>
        /// <para>1 = Data Entry View only</para>
        /// <para>2 = Proposal View only </para>
        /// </summary>
        public int DisplayState { get; set; } = 0;
        /// <summary>
        /// Is the proposal display visible
        /// </summary>
        public Visibility IsProposalDisplayVisible { get; set; } = Visibility.Visible;
        /// <summary>
        /// Is the tab control display (Data Entry View) visible
        /// </summary>
        public Visibility IsTabControlDisplayVisible { get; set; } = Visibility.Visible;
        /// <summary>
        /// The column number for the proposal display
        /// </summary>
        public int ProposalDisplayColumn { get; set; } = 1;
        /// <summary>
        /// The column span for the tab control
        /// </summary>
        public int TabControlColumnSpan { get; set; } = 1;
        /// <summary>
        /// The column span for the proposal display
        /// </summary>
        public int ProposalDisplayColumnSpan { get; set; } = 1;

        /// <summary>
        /// The application's job model - information about the job
        /// </summary>
        public Model.Job Job { get; set; } = new Model.Job();

        /// <summary>
        /// A collection of rooms
        /// </summary>
        public ObservableCollection<RoomVM> Rooms { get; set; } = new ObservableCollection<RoomVM>();

        /// <summary>
        /// Command to create a proposal
        /// </summary>
        public ICommand CreateProposalCommand { get; private set; }
        /// <summary>
        /// Command to load an existing job
        /// </summary>
        public ICommand LoadCommand { get; private set; }
        /// <summary>
        /// Command to add new room to the job
        /// </summary>
        public ICommand AddRoomCommand { get; private set; }
        /// <summary>
        /// Command to toggle the display's view
        /// </summary>
        public ICommand ToggleDisplayCommand { get; private set; }

        ICommand _RemoveCommand;
        /// <summary>
        /// Command to remove a room from the job (calls the 'Remove' Function)
        /// </summary>
        public ICommand RemoveCommand
        {
            get
            {
                if (_RemoveCommand == null)
                    _RemoveCommand = new CollectionChangeCommand(param => Remove((RoomVM)param));
                return _RemoveCommand;
            }
        }

        /// <summary>
        /// Removes a room from the job
        /// </summary>
        /// <param name="roomToDelete">The room that needs to be deleted</param>
        public void Remove(RoomVM roomToDelete)
        {
            if (MessageBox.Show("Deleting this room will delete all items contained in it. \nAre you sure you want to continue?", 
                "Warning", 
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (Rooms.Contains(roomToDelete))
                    Rooms.Remove(roomToDelete);
            }
        }

        /// <summary>
        /// Set the premise address to the mailing address
        /// </summary>
        bool _PremiseEqualsMailing;
        public bool PremiseEqualsMailing
        {
            get { return _PremiseEqualsMailing; }
            set
            {
                _PremiseEqualsMailing = value;
                OnPropertyChanged("PremiseEqualsMailing");
                if (value == true)
                {
                    Job.PremiseAddress01 = Job.MailingAddress01;
                    Job.PremiseAddress02 = Job.MailingAddress02;
                    Job.PremiseCity = Job.MailingCity;
                    Job.PremiseState = Job.MailingState;
                    Job.PremiseZip = Job.MailingZip;
                }
                else
                    Job.PremiseAddress01 = Job.PremiseAddress02 = Job.PremiseCity = Job.PremiseState = Job.PremiseZip = null;
            }
        }

        /// <summary>
        /// Creates a new instance of a job view model
        /// </summary>
        public JobVM()
        {
            // Initialize job commands
            CreateProposalCommand = new CreateProposalCommand(this);
            LoadCommand = new LoadCommand(this);
            AddRoomCommand = new AddRoomCommand(this);
            ToggleDisplayCommand = new ToggleDisplayCommand(this);
        }

       /// <summary>
       /// Adds a new room to the collection
       /// </summary>
       /// <param name="RoomName">The name of the room</param>
        public void AddRoom(string RoomName = null)
        {
            bool HasRoomName = false || RoomName != null;

            RoomVM roomVM = new RoomVM(HasRoomName ? RoomName : "Room" + CurrentRoomNumber, CurrentRoomNumber);
            roomVM.PropertyChanged += Room_PropertyChanged;

            Rooms.Add(roomVM);

            CurrentRoomNumber++;
        }

        /// <summary>
        /// Creates the job's proposal
        /// </summary>
        public void CreateProposal()
        {
            SaveFile savefile = new SaveFile();
            savefile.Save(this);
            MessageBox.Show("Saved!");
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
       
        void Room_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                // The room's total price changes
                case "TotalPrice":
                    TotalPrice = 0;
                    foreach (RoomVM room in Rooms)
                        TotalPrice += room.TotalPrice;
                    break;

                // The room's shelving cost changes
                case "ShelvingCost":
                    ShelvingCost = 0;
                    foreach (RoomVM room in Rooms)
                        ShelvingCost += room.ShelvingCost;
                    break;

                // The room's strip cost changes
                case "StripCost":
                    StripCost = 0;
                    foreach (RoomVM room in Rooms)
                        StripCost += room.StripCost;
                    break;

                // The room's accessory costs changes
                case "AccessoryCost":
                    AccessoryCost = 0;
                    foreach (RoomVM room in Rooms)
                        AccessoryCost += room.AccessoryCost;
                    break;
            }
        }
        #endregion
    }
}
