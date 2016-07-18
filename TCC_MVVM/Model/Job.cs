using System;
using System.ComponentModel;
using PropertyChanged;

namespace TCC_MVVM.Model
{
    [ImplementPropertyChanged]
    public class Job : INotifyPropertyChanged, IDataErrorInfo
    {
        //======================================
        // Contact Information Variables
        //======================================
        public string FirstName { get; set; } = "Tyler";
        public string FullName { get; set; } = "Tyler Holstead";
        public string PhoneNumber { get; set; } = "(920) 857-4544";
        public string Email { get; set; }
        public string MailingAddress01 { get; set; } = "1471 Navigator Way";
        public string MailingAddress02 { get; set; }
        public string MailingCity { get; set; }
        public string MailingState { get; set; }
        public string MailingZip { get; set; }

        //======================================
        // Premise Information Variables
        //======================================
        public string PremiseAddress01 { get; set; }
        public string PremiseAddress02 { get; set; }
        public string PremiseCity { get; set; }
        public string PremiseState { get; set; }
        public string PremiseZip { get; set; }
        public string[] ProjectTypeValues { get; set; } = { "New Construction", "Remodel", "Other" };
        public string ProjectType { get; set; }
        public double Distance { get; set; }

        //======================================
        // Job Information Variables
        //======================================
        public int NumRooms { get; set; } = 1;
        public string ProposalOutputPath { get; set; }
        public string ItemListOutputPath { get; set; }

        /// <summary>
        /// A list of property names, which correspond to the actual name of the property
        /// </summary>
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

        /// <summary>
        /// Set each property by their property name
        /// </summary>
        /// <param name="PropertyName">
        /// The name of the property
        /// </param>
        /// <param name="PropertyValue">
        /// The value of the property
        /// </param>
        public void SetProperty(string PropertyName, string PropertyValue)
        {
            switch(PropertyName)
            {
                case "FullName": FullName = PropertyValue; break;
                case "PhoneNumber": PhoneNumber = PropertyValue; break;
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

        /// <summary>
        /// Get the value of a property by their property name
        /// </summary>
        /// <param name="PropertyName">
        /// The name of the property
        /// </param>
        /// <returns>
        /// The value of that property
        /// </returns>
        public object GetPropertyValue(string PropertyName)
        {
            switch(PropertyName)
            {
                case "FullName": return FirstName;
                case "PhoneNumber": return PhoneNumber;
                case "Email": return Email;
                case "MailingAddress01": return MailingAddress01;
                case "MailingAddress02": return MailingAddress02;
                case "MailingCity": return MailingCity;
                case "MailingState": return MailingState;
                case "MailingZip": return MailingZip;
                case "PremiseAddress01": return PremiseAddress01;
                case "PremiseAddress02": return PremiseAddress02;
                case "PremiseCity": return PremiseCity;
                case "PremiseState": return PremiseState;
                case "PremiseZip": return PremiseZip;
                case "NumRooms": return NumRooms;
                case "Distance": return Distance;
            }
            return null;
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
                foreach(string property in ValidateProperties)
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
