using BookingApp.Dto;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.UseCases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using System.Windows.Media.Imaging;

namespace BookingApp.ViewModel.GuestViewModel
{
    public class OwnerRatingViewModel
    {
        public PropertyReservation SelectedReservation { get; set; }
        public Property SelectedProperty { get; set; }
        public Guest LoggedInGuest { get; set; }
        public OwnerReviewDto OwnerReview { get; set; }
        public OwnerReviewRepository OwnerReviewRepository { get; set; }
        public ImageService ImageService { get; set; }
        public int ImageIndex { get; set; }

        private ObservableCollection<BitmapImage> images = new ObservableCollection<BitmapImage>();
        public List<string> ImagesPaths { get; set; }
        public List<string> AbsolutePaths { get; set; }

        private string showingImage;
    }
}
