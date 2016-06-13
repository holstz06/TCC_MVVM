using System;
using System.Windows;
using System.Windows.Input;

namespace TCC_MVVM.ViewModel.Job
{
    public class CreateJobCommand : ICommand
    {
        private JobVM ViewModel;
        private Model.Job Job;
        public CreateJobCommand(JobVM ViewModel)
        {
            this.ViewModel = ViewModel;
            Job = this.ViewModel.Job;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModel.Rooms.Clear();
            bool validModel = ViewModel.Job.IsValid;
            if (validModel)
            {
                for (int i = 1; i <= Job.NumRooms; i++)
                {
                    ViewModel.AddRoom();
                }
            }
            else
                MessageBox.Show("Please make sure all required fields are entered.");
        }
    }
}
