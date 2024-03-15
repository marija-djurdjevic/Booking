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
    public class ReviewDto : INotifyPropertyChanged
    {
        private int id;
        private int reservationId;
        private int guestId;
        private int cleanliness;
        private int rules;
        private string comment;

        public ReviewDto() { }

        public ReviewDto(int reservationId, int guestId, int cleanliness, int rules, string comment)
        {
            this.reservationId = reservationId;
            this.guestId = guestId;
            this.cleanliness = cleanliness;
            this.rules = rules;
            this.comment = comment;
         }

        public ReviewDto(Review review)
        {
            reservationId = review.ReservationId;
            guestId = review.GuestId;
            cleanliness = review.Cleanliness;
            rules = review.Rules;
            comment = review.Comment;
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
        public int Cleanliness
        {

            get { return cleanliness; }
            set
            {
                if (value != cleanliness)
                {
                    cleanliness = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Rules
        {

            get { return rules; }
            set
            {
                if (value != rules)
                {
                    rules = value;
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

        public Review ToReview()
        {
            return new Review(ReservationId, GuestId, Cleanliness, Rules, Comment);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}

