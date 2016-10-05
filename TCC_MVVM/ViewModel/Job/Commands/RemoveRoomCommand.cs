using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TCC_MVVM.ViewModel.Job.Commands
{
    class RemoveRoomCommand : ICommand
    {
        JobVM ViewModel;
        public RemoveRoomCommand(JobVM ViewModel)
        {
            this.ViewModel = ViewModel;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            int roomnumber = (int)parameter;

            // Open dialog to verify user's action
            switch(MessageBox.Show("Removing this room will delete all shelving added to it. Continue?", "Continue?", MessageBoxButton.YesNo))
            {
                case MessageBoxResult.Yes:
                    foreach(Room.RoomVM roomvm in ViewModel.Rooms)
                    {
                        if (roomnumber == roomvm.Room.RoomNumber)
                            ViewModel.Rooms.Remove(roomvm);
                    }
                    break;
                case MessageBoxResult.No: break;
            }
        }
    }
}
