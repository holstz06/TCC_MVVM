using System.Linq;
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

        public Room(string RoomName, int RoomNumber)
        {
            this.RoomName = RoomName;
            this.RoomNumber = RoomNumber;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
