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
                {
                    Job.PremiseAddress01 = null;
                    Job.PremiseAddress02 = null;
                    Job.PremiseCity = null;
                    Job.PremiseState = null;
                    Job.PremiseZip = null;
                }
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
        }

        /// <summary>
        /// Adds a new room to the collection
        /// </summary>
        /// <param name="RoomName"></param>
        public void AddRoom(string RoomName = null)
        {
            bool HasRoomName = false || RoomName != null;

            RoomVM roomVM = new RoomVM(HasRoomName ? RoomName : "Room" + CurrentRoomNumber, CurrentRoomNumber);
            roomVM.PropertyChanged += RoomPropertyChanged;

            Rooms.Add(roomVM);

            CurrentRoomNumber++;
        }

        /// <summary>
        /// Creates a proposal based on this job
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

        void RoomPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "TotalPrice")
            {
                TotalPrice = 0;
                foreach(RoomVM room in Rooms)
                {
                    TotalPrice += room.TotalPrice;
                }
            }
            if(e.PropertyName == "ShelvingCost")
            {
                ShelvingCost = 0;
                foreach(RoomVM room in Rooms)
                {
                    ShelvingCost += room.ShelvingCost;
                }
            }
            if(e.PropertyName == "StripCost")
            {
                StripCost = 0;
                foreach(RoomVM room in Rooms)
                {
                    StripCost += room.StripCost;
                }
            }
            if(e.PropertyName == "AccessoryCost")
            {
                AccessoryCost = 0;
                foreach(RoomVM room in Rooms)
                {
                    AccessoryCost += room.AccessoryCost;
                }
            }
        }
        #endregion

        public RoomVM RoomVM
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }
    }
}
