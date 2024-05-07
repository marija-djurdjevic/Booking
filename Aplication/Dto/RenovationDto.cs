using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models;
using System.ComponentModel;
using System.Data;


namespace BookingApp.Aplication.Dto
{
    public class RenovationDto : INotifyPropertyChanged
    {
        private int id;
        private int ownerId;
        private int propertyId;
        private DateTime startDate;
        private DateTime endDate;
        private string description;
        private int duration;

        public RenovationDto() { }
        public RenovationDto(int id, int ownerId, int propertyId, DateTime startDate, DateTime endDate, string description, int duration)
        {
            this.id = id;
            this.ownerId = ownerId;
            this.propertyId = propertyId;
            this.startDate = startDate;
            this.endDate = endDate;
            this.description = description;
            this.duration = duration;
        }
        public RenovationDto(int ownerId, int propertyId, DateTime startDate, DateTime endDate, string description, int duration)
        {
            this.ownerId = ownerId;
            this.propertyId = propertyId;
            this.startDate = startDate;
            this.endDate = endDate;
            this.description = description;
            this.duration = duration;
        }
        public RenovationDto(Renovation renovation)
        {
            ownerId = renovation.OwnerId;
            propertyId = renovation.PropertyId;
            startDate = renovation.StartDate;
            endDate = renovation.EndDate;
            description = renovation.Description;
            duration = renovation.Duration;

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
        public string Description
        {

            get { return description; }
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Duration
        {

            get { return duration; }
            set
            {
                if (value != duration)
                {
                    duration = value;
                    OnPropertyChanged();
                }
            }
        }
        public Renovation ToRenovation()
        {
            return new Renovation(OwnerId, PropertyId, StartDate, EndDate, Description, Duration);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
