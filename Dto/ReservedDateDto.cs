using BookingApp.DTO;
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
    public class ReservedDateDto : INotifyPropertyChanged
    {
        private int propertyId;
        private int reservationId;
        private DateTime datevalue;


        public ReservedDateDto() { }

        public ReservedDateDto(int idProperty, DateTime datevalue, int reservationId)
        {
            this.propertyId = idProperty;
            this.datevalue = datevalue;
            this.reservationId = reservationId;
        }

        public ReservedDateDto(ReservedDate reservedDate)
        {
            propertyId = reservedDate.PropertyId;
            datevalue = reservedDate.Value;
            reservationId = reservedDate.ReservationId;

        }

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

        public DateTime Value
        {
            get { return datevalue; }
            set
            {
                if (value != datevalue)
                {
                    datevalue = value;
                    OnPropertyChanged();
                }

            }
        }

        public ReservedDate ToReservedDate()
        {
            return new ReservedDate(PropertyId, datevalue, reservationId);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
