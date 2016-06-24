using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TCC_MVVM.ViewModel.Room;
using PropertyChanged;
using TCC_MVVM.Model;

namespace TCC_MVVM.ViewModel.Job
{
    [ImplementPropertyChanged]
    public class JobVM : INotifyPropertyChanged
    {
        private int CurrentRoomNumber { get; set; } = 0;

        public decimal TotalPrice { get; set; }

        public Model.Job Job { get; set; } = new Model.Job();

        public ObservableCollection<RoomVM> Rooms { get; set; } 
            = new ObservableCollection<RoomVM>();

        public ICommand BrowseCommand { get; private set; }
        public ICommand CreateJobCommand { get; private set; }
        public ICommand CreateProposalCommand { get; private set; }
        public ICommand SamePremiseAddressCommand { get; private set; }

        /// <summary>
        /// Set the premise address to the mailing address
        /// </summary>
        private bool _PremiseEqualsMailing = false;
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
            BrowseCommand = new BrowseCommand(this);
            CreateJobCommand = new CreateJobCommand(this);
            SamePremiseAddressCommand = new SamePremiseAddressCommand(this);
            CreateProposalCommand = new CreateProposalCommand(this);

            //AddRoom("Master Bedroom");
        }

        /// <summary>
        /// Adds a new room to the collection
        /// </summary>
        /// <param name="RoomName">
        /// (Optional) The name of the room. A default name will be given if one is not provided
        /// </param>
        public void AddRoom(string RoomName = null)
        {
            bool HasRoomName = false;
            if (RoomName != null) HasRoomName = true;

            RoomVM roomVM = new RoomVM(HasRoomName ? RoomName : "Room" + CurrentRoomNumber.ToString(), CurrentRoomNumber);
            roomVM.PropertyChanged += RoomPropertyChanged;

            Rooms.Add(roomVM);

            CurrentRoomNumber++;
        }

        /// <summary>
        /// Creates a proposal based on this job
        /// </summary>
        public void CreateProposal()
        {
            Proposal proposal = new Proposal(this) { ItemListPath = "C:\\Users\\tyhol\\Desktop\\Proposal1.xlsx" };
            proposal.CreateItemList();
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RoomPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "TotalPrice")
            {
                TotalPrice = 0;
                foreach(RoomVM room in Rooms)
                {
                    TotalPrice += room.TotalPrice;
                }
            }
        }
        #endregion
    }
}
