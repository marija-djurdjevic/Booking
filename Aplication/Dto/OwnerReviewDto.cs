using BookingApp.Domain.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookingApp.Aplication.Dto
{
    public class OwnerReviewDto : INotifyPropertyChanged
    {
        private int id;
        private int ownerId;
        private int reservationId;
        private int cleanliness;
        private int correctnes;
        private string comment;
        private List<string> imagesPaths = new List<string>();

        public OwnerReviewDto() { }

        public OwnerReviewDto(int ownerId, int reservationId, int cleanliness, int correctnes, string comment, List<string> imagesPaths)
        {
            this.ownerId = ownerId;
            this.reservationId = reservationId;
            this.cleanliness = cleanliness;
            this.correctnes = correctnes;
            this.comment = comment;
            this.imagesPaths = imagesPaths;
        }

        public OwnerReviewDto(OwnerReview ownerReview)
        {
            ownerId = ownerReview.OwnerId;
            reservationId = ownerReview.ReservationId;
            cleanliness = ownerReview.Cleanliness;
            correctnes = ownerReview.Correctness;
            comment = ownerReview.Comment;
            imagesPaths = ownerReview.ImagesPaths;
        }

        public OwnerReviewDto(int cleanliness, int correctnes, string comment)
        {
            this.cleanliness = cleanliness;
            this.correctnes = correctnes;
            this.comment = comment;
        }

        public List<string> ImagesPaths
        {
            get { return imagesPaths; }
            set
            {
                if (value != imagesPaths)
                {
                    imagesPaths = value;
                    OnPropertyChanged();
                }
            }

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
        public int Correctness
        {

            get { return correctnes; }
            set
            {
                if (value != correctnes)
                {
                    correctnes = value;
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

        public OwnerReview ToOwnerReview()
        {
            return new OwnerReview(OwnerId, ReservationId, Cleanliness, Correctness, Comment, ImagesPaths);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}

