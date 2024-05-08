using BookingApp.Domain.Models;
using BookingApp.WPF.Views.GuestView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookingApp.Aplication.Dto
{
    public class RenovationReccomendationDto : INotifyPropertyChanged
    {
        private int guestId { get; set; }
        private int ownerReviewId {  get; set; }
        private int urgencyLevel {  get; set; }
        private string comment {  get; set; }
        public int Id { get; set; }
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

        public int OwnerReviewId
        {
            get { return ownerReviewId; }
            set
            {
                if (value != ownerReviewId)
                {
                    ownerReviewId = value;
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
        public int UrgencyLevel
        {
            get { return urgencyLevel; }
            set
            {
                if (value != urgencyLevel)
                {
                    urgencyLevel = value;
                    OnPropertyChanged();
                }
            }
        }

        public Domain.Models.RenovationReccomendation ToRenovationReccomendation()
        {
            return new Domain.Models.RenovationReccomendation(guestId, ownerReviewId, urgencyLevel, comment);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public RenovationReccomendationDto()
        {
        }

        public RenovationReccomendationDto(int guestId, int ownerReviewId, int urgencyLevel, string comment)
        {
            this.guestId = guestId;
            this.ownerReviewId = ownerReviewId;
            this.urgencyLevel = urgencyLevel;
            this.comment = comment;
        }

    }
}
