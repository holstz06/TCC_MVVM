using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TCC_MVVM.ViewModel.Job
{
    class LoadCommand : ICommand
    {
        JobVM ViewModel;
        public LoadCommand(JobVM ViewModel)
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
            Model.SaveFile savefile = new Model.SaveFile();
            JobVM tempViewModel = savefile.Load();
            ViewModel.Job = tempViewModel.Job;
            ViewModel.Rooms = tempViewModel.Rooms;
        }
    }
}
