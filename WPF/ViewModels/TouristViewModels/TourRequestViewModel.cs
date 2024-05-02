using BookingApp.Domain.Models.Enums;
using BookingApp.Domain.Models;
using BookingApp.View.GuideView;
using BookingApp.WPF.ViewModels.TouristViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.WPF.Validations;

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class TourRequestViewModel : BindableBase, IDataErrorInfo
    {
        private int id;
        private int guideId;
        private int touristId;
        private Location location;
        private string description;
        private string language;
        private int touristNumber;
        private DateTime startDate;
        private DateTime endDate;
        private List<Tuple<string, string, int>> persons;
        private TourRequestStatus status;
        private DateTime acceptedDate;
        private int complexId;

        public string Error => null;

        public TourRequestViewModel()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            status = TourRequestStatus.Pending;
            Location = new Location();
            Persons = new List<Tuple<string, string, int>>();
        }
        //Verifikation
        public Verifications verifications = new Verifications();
        public string this[string columnName]
        {
            get
            {

                if (columnName == "Country")
                {
                    if (string.IsNullOrEmpty(Country))
                        return "Country is required";

                }
                if (columnName == "City")
                {
                    if (string.IsNullOrEmpty(City))
                        return "City is required";

                }
                if (columnName == "Language")
                {
                    if (string.IsNullOrEmpty(Language))
                        return "Language is required";

                }
                if (columnName == "StartDate")
                {
                    if (StartDate.Date < DateTime.Now.AddHours(48).Date)
                        return "Start date must be 2 days from now.";

                }
                if (columnName == "EndDate")
                {
                    if (EndDate.Date < DateTime.Now.AddHours(48).Date)
                        return "End date must be 2 days from now.";

                }
                if (columnName == "DateMessage")
                {
                    if (EndDate.Date < StartDate.Date && StartDate.Date >= DateTime.Now.AddHours(48).Date && EndDate.Date > DateTime.Now.AddHours(48).Date)
                        return "End date is less than the start date.";

                }
                if (columnName == "TouristNumber")
                {
                    if (TouristNumber < 1)
                        return "Must be positive number";

                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "DateMessage", "City", "Country", "Language", "StartDate", "EndDate", "TouristNumber" };

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
        public string DateMessage
        {
            get => StartDate.ToString();
            set
            {
                if (value != StartDate.ToString())
                {
                    DateMessage = value;
                    OnPropertyChanged(nameof(DateMessage));
                }
            }
        }

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
        public int ComplexId
        {
            get => complexId;
            set
            {
                if (value != complexId)
                {
                    id = value;
                    OnPropertyChanged(nameof(ComplexId));
                }
            }
        }
        public int GuideId
        {
            get => guideId;
            set
            {
                if (value != guideId)
                {
                    guideId = value;
                    OnPropertyChanged(nameof(GuideId));
                }
            }
        }
        public int TouristId
        {
            get => touristId;
            set
            {
                if (value != touristId)
                {
                    touristId = value;
                    OnPropertyChanged(nameof(TouristId));
                }
            }
        }
        public string Country
        {
            get => Location.Country;
            set
            {
                if (value != Location.Country)
                {
                    Location.Country = value;
                    OnPropertyChanged(nameof(Country));
                }
            }
        }
        public string City
        {
            get => Location.City;
            set
            {
                if (value != Location.City)
                {
                    Location.City = value;
                    OnPropertyChanged(nameof(City));
                }
            }
        }
        public Location Location
        {
            get => location;
            set
            {
                if (value != location)
                {
                    location = value;
                    OnPropertyChanged(nameof(Location));
                }
            }
        }
        public string Description
        {
            get => description;
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }
        public string Language
        {
            get => language;
            set
            {
                if (value != language)
                {
                    language = value;
                    OnPropertyChanged(nameof(Language));
                }
            }
        }
        public int TouristNumber
        {
            get => touristNumber;
            set
            {
                if (value != touristNumber)
                {
                    touristNumber = value;
                    OnPropertyChanged(nameof(TouristNumber));
                }
            }
        }
        public DateTime StartDate
        {
            get => startDate;
            set
            {
                if (value != startDate)
                {
                    startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                    OnPropertyChanged(nameof(DateMessage));
                }
            }
        }
        public DateTime EndDate
        {
            get => endDate;
            set
            {
                if (value != endDate)
                {
                    endDate = value;
                    OnPropertyChanged(nameof(DateMessage));
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }
        public List<Tuple<string, string, int>> Persons
        {
            get => persons;
            set
            {
                if (value != persons)
                {
                    persons = value;
                    OnPropertyChanged(nameof(Persons));
                }
            }
        }
        public TourRequestStatus Status
        {
            get => status;
            set
            {
                if (value != status)
                {
                    status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }
        public DateTime AcceptedDate
        {
            get => acceptedDate;
            set
            {
                if (value != acceptedDate)
                {
                    acceptedDate = value;
                    OnPropertyChanged(nameof(AcceptedDate));
                }
            }
        }
        public TourRequestViewModel(TourRequest tourRequest)
        {
            Id = tourRequest.Id;
            GuideId = tourRequest.GuideId;
            TouristId = tourRequest.TouristId;
            Location = tourRequest.Location;
            Description = tourRequest.Description;
            Language = tourRequest.Language;
            TouristNumber = tourRequest.TouristNumber;
            StartDate = tourRequest.StartDate;
            EndDate = tourRequest.EndDate;
            Persons = tourRequest.Persons;
            Status = tourRequest.Status;
            AcceptedDate = tourRequest.AcceptedDate;
            ComplexId = tourRequest.ComplexId;
        }
        public TourRequest ToTourRequest()
        {
            return new TourRequest(Id, GuideId,ComplexId, TouristId, Location, Description, Language, TouristNumber, StartDate, EndDate, Persons, Status, AcceptedDate);
        }

    }
}
