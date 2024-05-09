using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BookingApp.WPF.ViewModels.GuestViewModels
{
    public class GuestReviewScoreViewModel
    {
        public Guest LoggedInGuest { get; set; }
        public int Reviews { get; set; }
        public double AvgCleanliness { get; set; }
        public double AvgRespectingRules { get; set; }
        public List<Review> AllGuestReviews { get; set; }
        public List<Review> GuestReviews { get; set; }
        public List<OwnerReview> OwnerReviews { get; set; }
        public GuestReviewService guestReviewService;
        public ObservableCollection<KeyValuePair<Review, PropertyReservation>> PropertiesReviews { get; set; }
   
        public GuestReviewScoreViewModel(Guest guest){
            guestReviewService = new GuestReviewService(Injector.CreateInstance<IReviewRepository>(), Injector.CreateInstance<IPropertyReservationRepository>(), Injector.CreateInstance<IOwnerReviewRepository>());
            LoggedInGuest = guest;
            OwnerReviews = new List<OwnerReview>();
            AllGuestReviews = new List<Review>();
            GuestReviews = new List<Review>();
            PropertiesReviews = new ObservableCollection<KeyValuePair<Review, PropertyReservation>>();
            GuestReviews = guestReviewService.FindAAppropriateReviews(AllGuestReviews, OwnerReviews);
            Reviews = GuestReviews.Count();
            AvgCleanliness = guestReviewService.CalculateAvarageCleanlinessScore(GuestReviews);
            AvgRespectingRules = guestReviewService.CalculateAvarageRulesScore(GuestReviews);
            guestReviewService.MakeReviewPairs(GuestReviews, PropertiesReviews);
        }
    }
}
