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
        int CurrentRoomNumber { get; set; } = 0;

        public decimal TotalPrice { get; set; }
        public decimal ShelvingCost { get; set; }
        public decimal StripCost { get; set; }
        public decimal AccessoryCost { get; set; }

        public int DisplayState { get; set; } = 0;
        public Visibility IsProposalDisplayVisible { get; set; } = Visibility.Visible;
        public Visibility IsTabControlDisplayVisible { get; set; } = Visibility.Visible;
        public int ProposalDisplayColumn { get; set; } = 1;
        public int TabControlColumnSpan { get; set; } = 1;
        public int ProposalDisplayColumnSpan { get; set; } = 1;

        public Model.Job Job { get; set; } = new Model.Job();

        public ObservableCollection<RoomVM> Rooms { get; set; } 
            = new ObservableCollection<RoomVM>();

        //==========================================================
        // Job Commands
        //==========================================================
        public ICommand BrowseCommand { get; private set; }
        public ICommand CreateJobCommand { get; private set; }
        public ICommand CreateProposalCommand { get; private set; }
        public ICommand SamePremiseAddressCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }
        public ICommand AddRoomCommand { get; private set; }
        
        public ICommand ToggleDisplayCommand { get; private set; }

        ICommand _RemoveCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (_RemoveCommand == null)
                    _RemoveCommand = new CollectionChangeCommand(param => Remove((RoomVM)param));
                return _RemoveCommand;
            }
        }

        public void Remove(RoomVM roomToDelete)
        {
            if (MessageBox.Show("Deleting this room will delete all items contained in it. \nAre you sure you want to continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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

        public JobVM()
        {
            // Initialize job commands
            BrowseCommand = new BrowseCommand(this);
            CreateJobCommand = new CreateJobCommand(this);
            SamePremiseAddressCommand = new SamePremiseAddressCommand(this);
            CreateProposalCommand = new CreateProposalCommand(this);
            LoadCommand = new LoadCommand(this);
            AddRoomCommand = new AddRoomCommand(this);
            ToggleDisplayCommand = new ToggleDisplayCommand(this);
        }

        ///===============================================================================
        /// <summary>
        ///     Adds a new room to the collection
        /// </summary>
        /// 
        /// <param name="RoomName">
        ///     (Optional) The name of the room
        /// </param>
        ///===============================================================================
        public void AddRoom(string RoomName = null)
        {
            bool HasRoomName = false || RoomName != null;

            RoomVM roomVM = new RoomVM(HasRoomName ? RoomName : "Room" + CurrentRoomNumber, CurrentRoomNumber);
            roomVM.PropertyChanged += Room_PropertyChanged;

            Rooms.Add(roomVM);

            CurrentRoomNumber++;
        }

        ///===============================================================================
        /// <summary>
        ///     Creates a proposal based on this job
        /// </summary>
        ///===============================================================================
        public void CreateProposal()
        {
            SaveFile savefile = new SaveFile();
            savefile.Save(this);
            MessageBox.Show("Saved!");
        }


        public event PropertyChangedEventHandler PropertyChanged;

        ///===============================================================================
        /// <summary>
        ///     Method is called by the 'set' accessory of each property
        /// </summary>
        ///===============================================================================
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        

        ///===============================================================================
        /// <summary>
        ///     Property changed event handeler for 'room model'. Once subscribed, the
        ///     view model is aware of all the room's property changes
        /// </summary>
        ///===============================================================================
        void Room_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TotalPrice":
                    TotalPrice = 0;
                    foreach (RoomVM room in Rooms)
                        TotalPrice += room.TotalPrice;
                    break;

                case "ShelvingCost":
                    ShelvingCost = 0;
                    foreach (RoomVM room in Rooms)
                        ShelvingCost += room.ShelvingCost;
                    break;

                case "StripCost":
                    StripCost = 0;
                    foreach (RoomVM room in Rooms)
                        StripCost += room.StripCost;
                    break;

                case "AccessoryCost":
                    AccessoryCost = 0;
                    foreach (RoomVM room in Rooms)
                        AccessoryCost += room.AccessoryCost;
                    break;
            }
        }
    }
}
