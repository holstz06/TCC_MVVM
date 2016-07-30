using System;
using System.ComponentModel;
using PropertyChanged;
using System.Collections.Generic;
using System.Linq;

namespace TCC_MVVM.Model
{
    [ImplementPropertyChanged]
    public class Job : INotifyPropertyChanged, IDataErrorInfo
    {
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string MailingAddress01 { get; set; }
        public string MailingAddress02 { get; set; }
        public string MailingCity { get; set; }
        public string MailingState { get; set; }
        public string MailingZip { get; set; }
        public string PremiseAddress01 { get; set; }
        public string PremiseAddress02 { get; set; }
        public string PremiseCity { get; set; }
        public string PremiseState { get; set; }
        public string PremiseZip { get; set; }
        public string[] ProjectTypeValues { get; set; } = { "New Construction", "Remodel", "Other" };
        public string ProjectType { get; set; }
        public double Distance { get; set; }
        public int NumRooms { get; set; } = 1;
        public string ProposalOutputPath { get; set; }
        public string ItemListOutputPath { get; set; }


        ///=================================================================================================
        /// <summary>
        ///     Queries the reflection of shelf class to gather a list of properties by their name
        /// </summary>
        /// 
        /// <returns>
        ///     A list of properties by their names
        /// </returns>
        ///=================================================================================================
        public List<string> GetPropertyNames()
            => GetType().GetProperties().Select(row => row.Name).ToList();

        //public void SetProperty(string PropertyName, string PropertyValue)
        //{
        //    foreach(var propertyName in GetPropertyNames())
        //    {
        //        if(PropertyName == propertyName)
        //    }
        //}

        public readonly string[] Properties =
        {
            "FullName",
            "PhoneNumber",
            "Email",
            "MailingAddress01",
            "MailingAddress02",
            "MailingCity",
            "MailingState",
            "MailingZip",
            "PremiseAddress01",
            "PremiseAddress02",
            "PremiseCity",
            "PremiseState",
            "PremiseZip",
            "NumRooms",
            "Distance"
        };

        public void SetProperty(string PropertyName, string PropertyValue)
        {
            switch (PropertyName)
            {
                case "FullName": FullName = PropertyValue; break;
                case "PhoneNumber": PhoneNumber = PropertyValue; break;
                case "Email": Email = PropertyValue; break;
                case "NumRooms": NumRooms = int.Parse(PropertyValue); break;
                case "MailingAddress01": MailingAddress01 = PropertyValue; break;
                case "MailingAddress02": MailingAddress02 = PropertyValue; break;
                case "MailingCity": MailingCity = PropertyValue; break;
                case "MailingState": MailingState = PropertyValue; break;
                case "MailingZip": MailingZip = PropertyValue; break;
                case "PremiseAddress01": PremiseAddress01 = PropertyValue; break;
                case "PremiseAddress02": PremiseAddress02 = PropertyValue; break;
                case "PremiseCity": PremiseCity = PropertyValue; break;
                case "PremiseState": PremiseState = PropertyValue; break;
                case "PremiseZip": PremiseZip = PropertyValue; break;
                case "Distance": Distance = double.Parse(PropertyValue); break;
            }
        }

        #region IDataErrorInfo Members

        string IDataErrorInfo.Error
        {
            get { return null; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get { return GetValidationError(propertyName); }
        }

        #endregion

        #region Property Member Validation

        /// <summary>
        /// Properties that need to be validated
        /// </summary>
        static readonly string[] ValidateProperties =
        {
            "NumRooms",
            "ExcelAppPath",
            "FullName",
            "MailingAddress01",
            "PhoneNumber"
        };

        /// <summary>
        /// Check the properties for validation (true = is valid, false = is not valid)
        /// </summary>
        public bool IsValid
        {
            get
            {
                foreach (string property in ValidateProperties)
                    if (GetValidationError(property) != null)
                        return false;
                return true;
            }
        }

        public string this[string PropertyName]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        string ValidateNumRooms()
        {
            if (NumRooms < 1)
                return "One or more rooms is needed";
            return null;
        }

        string ValidateFullName()
        {
            if (string.IsNullOrEmpty(FirstName))
                return "Name cannot be left blank";
            return null;
        }

        string ValidateMailingAddress()
        {
            if (string.IsNullOrEmpty(MailingAddress01))
                return "Mailing address cannot be left blank";
            return null;
        }

        string ValidatePhoneNumber()
        {
            if (string.IsNullOrEmpty(PhoneNumber))
                return "Phone number cannot be left blank";
            return null;
        }

        string GetValidationError(string propertyName)
        {
            string error = null;

            switch (propertyName)
            {
                case "NumRooms": error = ValidateNumRooms(); break;
                case "FullName": error = ValidateFullName(); break;
                case "MailingAddress01": error = ValidateMailingAddress(); break;
                case "PhoneNumber": error = ValidatePhoneNumber(); break;
            }

            return error;
        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
