using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TCC_MVVM.ViewModel.Job
{
    class SamePremiseAddressCommand : ICommand
    {
        private JobVM ViewModel;
        private Model.Job Job;
        public SamePremiseAddressCommand(JobVM ViewModel)
        {
            this.ViewModel = ViewModel;
            Job = ViewModel.Job;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true; // No instance where this cannot be ran
        }

        public void Execute(object parameter)
        {
            bool isChecked = ViewModel.PremiseEqualsMailing;
            bool isValid = false;

            // Only fill out if mailing address is set
            if(!string.IsNullOrEmpty(Job.MailingAddress01)
                && !string.IsNullOrEmpty(Job.MailingCity)
                && !string.IsNullOrEmpty(Job.MailingZip))
            {
                Job.PremiseAddress01 = Job.MailingAddress01;
                Job.PremiseAddress02 = Job.PremiseAddress02;
                Job.PremiseCity = Job.MailingCity;
                Job.PremiseState = Job.MailingState;
                Job.PremiseZip = Job.MailingZip;
            }
            else
            {
                MessageBox.Show("Please make sure the mailing address, city, state, or zip is not blank.");
            }

            if(isValid)
            {
                if(isChecked)
                {

                }
            }
        }
    }
}
