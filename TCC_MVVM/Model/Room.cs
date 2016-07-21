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
        public string ShelvingColor { get; set; }
        public string StripColor { get; set; }
        public string ShelvingDepth { get; set; }

        public ObservableCollection<string> WoodColorValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> StripColorValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> ShelvingDepthValues { get; set; } = new ObservableCollection<string>();

        public Room()
        {
            
        }

        public Room(string RoomName, int RoomNumber)
        {
            this.RoomName = RoomName;
            this.RoomNumber = RoomNumber;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public readonly string[] Properties = 
        {
            "RoomNumber",
            "RoomName",
            "ShelvingColor",
            "StripColor",
            "ShelvingDepth"
        };

        public void SetProperty(string PropertyName, string PropertyValue)
        {
            switch(PropertyName)
            {
                case "RoomNumber": RoomNumber = int.Parse(PropertyValue); break;
                case "RoomName": RoomName = PropertyValue; break;
                case "ShelvingColor": ShelvingColor = PropertyValue; break;
                case "StripColor": StripColor = PropertyValue; break;
                case "ShelvingDepth": ShelvingDepth = PropertyValue; break;

            }
        }
    }
}
