using BookingApp.Domain.Models;
using BookingApp.WPF.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class TourReservationViewModel : BindableBase, IDataErrorInfo
    {
        private int id;
        private int tourId;
        private int userId;
        private string touristFirstName;
        private string touristLastName;
        private int touristAge;
        private KeyPoint joinedKeyPoint;
        private bool isOnTour;
        private bool isUser;
        public string Error => null;
        public int Id
        {
            get => id;
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }
        public int TourId
        {
            get => tourId;
            set
            {
                if (value != tourId)
                {
                    tourId = value;
                    OnPropertyChanged(nameof(TourId));
                }
            }
        }
        public int UserId
        {
            get => userId;
            set
            {
                if (value != userId)
                {
                    userId = value;
                    OnPropertyChanged(nameof(UserId));
                }
            }
        }
        public string TouristFirstName
        {
            get => touristFirstName;
            set
            {
                if (value != touristFirstName)
                {
                    touristFirstName = value;
                    OnPropertyChanged(nameof(TouristFirstName));
                }
            }
        }
        public string TouristLastName
        {
            get => touristLastName;
            set
            {
                if (value != touristLastName)
                {
                    touristLastName = value;
                    OnPropertyChanged(nameof(TouristLastName));
                }
            }
        }
        public int TouristAge
        {
            get => touristAge;
            set
            {
                if (value != touristAge)
                {
                    touristAge = value;
                    OnPropertyChanged(nameof(TouristAge));
                }
            }
        }
        public KeyPoint JoinedKeyPoint
        {
            get => joinedKeyPoint;
            set
            {
                if (value != joinedKeyPoint)
                {
                    joinedKeyPoint = value;
                    OnPropertyChanged(nameof(JoinedKeyPoint));
                }
            }
        }
        public bool IsOnTour
        {
            get => isOnTour;
            set
            {
                if (value != isOnTour)
                {
                    isOnTour = value;
                    OnPropertyChanged(nameof(IsOnTour));
                }
            }
        }
        public bool IsUser
        {
            get => isUser;
            set
            {
                if (value != isUser)
                {
                    isUser = value;
                    OnPropertyChanged(nameof(IsUser));
                }
            }
        }

        //Verifikation
        public Verifications verifications = new Verifications();
        public string this[string columnName]
        {
            get
            {

                if (columnName == "TouristFirstName")
                {
                    if (string.IsNullOrEmpty(TouristFirstName))
                        return "First name is required";

                }
                if (columnName == "TouristLastName")
                {
                    if (string.IsNullOrEmpty(TouristLastName))
                        return "Last name is required";

                }
                if (columnName == "TouristAge")
                {
                    if (TouristAge < 1)
                        return "Age must be positive number";

                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "TouristFirstName", "TouristLastName", "TouristAge" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }
        //End of verification
        public TourReservationViewModel(TourReservation tourReservation)
        {
            Id = tourReservation.Id;
            TourId = tourReservation.TourId;
            UserId = tourReservation.UserId;
            TouristFirstName = tourReservation.TouristFirstName;
            TouristLastName = tourReservation.TouristLastName;
            TouristAge = tourReservation.TouristAge;
            JoinedKeyPoint = tourReservation.JoinedKeyPoint;
            IsOnTour = tourReservation.IsOnTour;
            IsUser = tourReservation.IsUser;
        }
        public TourReservation ToTourReservation()
        {
            return new TourReservation(Id, TourId, UserId, TouristFirstName, TouristLastName, TouristAge, JoinedKeyPoint, IsOnTour, IsUser);
        }

    }
}
