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
        private string propertyName { get; set; } 
        private int guests { get; set; }
        private int days { get; set; }
        private int guestId { get; set; }
        private int ownerId {  get; set; }
        private string guestFirstName { get; set; }
        private string guestLastName { get; set; }
        private DateTime startDate { get; set; }
        private DateTime endDate { get; set; }
        private bool canceled {  get; set; }

        public int Id { get; set; }
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

        public bool Canceled
        {
            get { return canceled; }
            set
            {
                if (value != canceled)
                {
                    canceled = value;
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

        public string PropertyName
        {
            get { return propertyName; }
            set
            {
                if (value != propertyName)
                {
                    propertyName = value;
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

        public int OwnerId
        {
            get { return ownerId; }
            set
            {
                if (value != ownerId)
                {
                    ownerId = value;
                    OnPropertyChanged();
                }
            }
        }

        public PropertyReservation ToPropertyReservation()
        {
            return new PropertyReservation(propertyId, guests, days, guestId, guestFirstName, guestLastName, startDate, endDate, propertyName, canceled, ownerId);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PropertyReservationDto() { 
            canceled = false;
        }

        public PropertyReservationDto(int guests, int days, int propertyId, int guestId, string guestFirstName, string guestLastName, string propertyName, bool canceled, int ownerId)
        {
            this.propertyId = propertyId;
            this.propertyName = propertyName;
            this.guestId = guestId;
            this.guests = guests;
            this.days = days;
            this.guestFirstName = guestFirstName;
            this.guestLastName = guestLastName;
            this.propertyName = propertyName;
            this.canceled = canceled;
            this.ownerId = ownerId;
        }

    }
    
    
}
