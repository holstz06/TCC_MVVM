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
        /// <summary>
        /// The types of room that a closet could be
        /// </summary>
        public enum RoomTypes { ReachIn, WalkIn, Garage, Pantry, Office, MurphyBed, Laundry }

        public int RoomNumber { get; set; }
        public string RoomName { get; set; }
        public string RoomColor { get; set; }
        public string StripColor { get; set; }
        public string RoomDepth { get; set; }
        public RoomTypes RoomType { get; set; }

        public ObservableCollection<string> WoodColorValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> StripColorValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> RoomDepthValues { get; set; } = new ObservableCollection<string>();

        public DataTable Data { get; set; }

        public Room(DataTable Data, string RoomName = null, int RoomNumber = 0)
        {
            this.Data = Data;
            this.RoomName = RoomName;
            this.RoomNumber = RoomNumber;

            SetRoomColorValues();
            SetRoomDepthValues();
            SetStripColorValues();
        }

        private void SetRoomDepthValues()
        {
            var query = (
                from room in Data.AsEnumerable()
                select room.Field<string>("RoomDepth")).Distinct();

            foreach (string row in query)
                RoomDepthValues.Add(row);
        }

        private void SetRoomColorValues()
        {
            var query = (
                from room in Data.AsEnumerable()
                select room.Field<string>("WoodColor")).Distinct();

            foreach (string row in query)
                WoodColorValues.Add(row);
        }
        
        private void SetStripColorValues()
        {
            var query = (
                from panel in Data.AsEnumerable()
                select panel.Field<string>("StripColor")).Distinct();

            foreach (string row in query)
                StripColorValues.Add(row);
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
