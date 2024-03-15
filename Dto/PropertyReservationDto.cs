using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Dto
{
    public class PropertyReservationDto : INotifyPropertyChanged
    {
        private int propertyId {  get; set; }
        private int guests { get; set; }
        private int days { get; set; }
        private int guestId { get; set; }
        private string guestFirstName { get; set; }
        private string guestLastName { get; set; }
        private DateTime startDate { get; set; }
        private DateTime endDate { get; set; }

        public int PropertyReservationId { get; set; }
        public int Guests
        {
            get { return guests; }
            set
            {
                if (value != guests)
                {
                    guests = value;
                    OnPropertyChanged();
                }
            }
        }
        public string GuestFirstName
        {
            get { return guestFirstName; }
            set
            {
                if (value != guestFirstName)
                {
                    guestFirstName = value;
                    OnPropertyChanged();
                }
            }
        }
        public string GuestLastName
        {
            get { return guestLastName; }
            set
            {
                if (value != guestLastName)
                {
                    guestLastName = value;
                    OnPropertyChanged();
                }
            }
        }


        public int Days
        {
            get { return days; }
            set
            {
                if (value != days)
                {
                    days = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                if (value != startDate)
                {
                    startDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                if (value != endDate)
                {
                    endDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public int PropertyId
        {
            get { return propertyId; }
            set
            {
                if (value != propertyId)
                {
                    propertyId = value;
                    OnPropertyChanged();
                }
            }
        }

        public int GuestId
        {
            get { return guestId; }
            set
            {
                if (value != guestId)
                {
                    guestId = value;
                    OnPropertyChanged();
                }
            }
        }
        public PropertyReservation ToPropertyReservation()
        {
            return new PropertyReservation(propertyId, guests, days, guestId, guestFirstName, guestLastName, startDate, endDate);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PropertyReservationDto() { }

        public PropertyReservationDto(int guests, int days, int propertyId, int guestId, string guestFirstName, string guestLastName)
        {
            this.propertyId = propertyId;
            this.guestId = guestId;
            this.guests = guests;
            this.days = days;
            this.guestFirstName = guestFirstName;
            this.guestLastName = guestLastName;
        }

    }
    
    
}
