using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data;
using PropertyChanged;

namespace TCC_MVVM.Model
{
    [ImplementPropertyChanged]
    public class Room : INotifyPropertyChanged
    {
        public int RoomNumber { get; set; }
        public string RoomName { get; set; }
        public string RoomColor { get; set; }
        public string StripColor { get; set; }
        public string ShelvingDepth { get; set; }

        public ObservableCollection<string> WoodColorValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> StripColorValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> ShelvingDepthValues { get; set; } = new ObservableCollection<string>();

        public DataTable Data { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Room()
        {
            
        }

        /// <summary>
        /// Creates a new instance of a Room
        /// </summary>
        /// <param name="RoomName"></param>
        /// <param name="RoomNumber"></param>
        public Room(string RoomName, int RoomNumber)
        {
            this.RoomName = RoomName;
            this.RoomNumber = RoomNumber;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
