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
    public class ReservationChangeRequestDto : INotifyPropertyChanged
    {
        private int reservationId { get; set; }
        private DateTime oldStartDate { get; set; }
        private DateTime oldEndDate { get; set; }
        private DateTime newStartDate { get; set; }
        private DateTime newEndDate { get; set; }
        private RequestStatus requestStatus { get; set; }
        private int guestId {  get; set; }
        private string propertyName {  get; set; }
        private string comment { get; set; }

        public int Id { get; set; }
        public int ReservationId
        {
            get { return reservationId; }
            set
            {
                if (value != reservationId)
                {
                    reservationId = value;
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

        public string Comment
        {
            get { return comment; }
            set
            {
                if (value != comment)
                {
                    comment = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime OldStartDate
        {
            get { return oldStartDate; }
            set
            {
                if (value != oldStartDate)
                {
                    oldStartDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime OldEndDate
        {
            get { return oldEndDate; }
            set
            {
                if (value != oldEndDate)
                {
                    oldEndDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime NewStartDate
        {
            get { return newStartDate; }
            set
            {
                if (value != newStartDate)
                {
                    newStartDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime NewEndDate
        {
            get { return newEndDate; }
            set
            {
                if (value != newEndDate)
                {
                    newEndDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public RequestStatus RequestStatus
        {
            get { return requestStatus; }
            set
            {
                if (value != requestStatus)
                {
                    requestStatus = value;
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

        public ReservationChangeRequest ToReservationChangeRequest()
        {
            return new ReservationChangeRequest(reservationId, oldStartDate, oldEndDate, newStartDate ,NewEndDate, RequestStatus, GuestId, PropertyName, comment);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ReservationChangeRequestDto() {
            RequestStatus = RequestStatus.Processing;
        }

        public ReservationChangeRequestDto(int reservationId, DateTime oldStartDate, DateTime oldEndDate, DateTime newStartDate, DateTime newEndDate, RequestStatus requestStatus, int guestId, string propertyName, string comment)
        {
            this.reservationId = reservationId;
            this.oldStartDate = oldStartDate;
            this.oldEndDate = oldEndDate;
            this.newStartDate = newStartDate;
            this.newEndDate = newEndDate;
            this.requestStatus = requestStatus;
            this.guestId = guestId;
            this.propertyName = propertyName;
            this.comment = comment;
        }

    }

}
