using BookingApp.Command;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.View;
using BookingApp.View.GuideView;
using Microsoft.Build.Framework;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Input;

namespace BookingApp.ViewModel.GuidesViewModel
{
    public class TourReviewViewModel : BaseViewModel
    {
        private readonly TouristService touristService;
        private readonly TouristExperienceService touristExperienceService;
        private readonly TourReservationService tourReservationService;
        private int tourId;
        private RelayCommand navigateBackCommand;
        private RelayCommand reportReviewCommand;
        private ObservableCollection<TouristExperience> touristExperiences;

        public TourReviewViewModel(int tourId)
        {
            this.tourId = tourId;
            touristService = new TouristService();
            touristExperienceService = new TouristExperienceService();
            tourReservationService = new TourReservationService();
            navigateBackCommand = new RelayCommand(ExecuteNavigateBack);
            reportReviewCommand = new RelayCommand(ExecuteReportReview);
            TouristExperiences = new ObservableCollection<TouristExperience>();
            LoadTouristExperiences();
        }

        private void LoadTouristExperiences()
        {
            var touristExperiences = touristExperienceService.GetTouristExperiencesForTour(tourId);

            foreach (var experience in touristExperiences)
            {
                var tourist = touristService.GetByUserId(experience.TouristId);
                experience.Tourist = tourist;

                var reservations = tourReservationService.GetByTourId(tourId);
                var reservation = reservations.FirstOrDefault(r => r.UserId == experience.TouristId);
                if (reservation != null)
                {
                    experience.ArrivalKeyPoint = reservation.JoinedKeyPoint;
                }
            }

            TouristExperiences = new ObservableCollection<TouristExperience>(touristExperiences);
            OnPropertyChanged();
        }

        public void RefreshList()
        {
            OnPropertyChanged();
        }


        public RelayCommand NavigateBackCommand
        {
            get { return navigateBackCommand; }
            set
            {
                if (navigateBackCommand != value)
                {
                    navigateBackCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private void ExecuteNavigateBack()
        {
            var mainPage = new GuideMainPage1();
            GuideMainWindow.MainFrame.Navigate(mainPage);

        }
        public RelayCommand ReportReviewCommand
        {
            get
            {
                if (reportReviewCommand == null)
                {
                    reportReviewCommand = new RelayCommand(ExecuteReportReview);
                }
                return reportReviewCommand;
            }
        }

        private void ExecuteReportReview(object parameter)
        {

            if (parameter is TouristExperience experience)
            {
                experience.CommentStatus = "Invalid";
                touristExperienceService.Update(experience);

                LoadTouristExperiences();
            }
        }

        public ObservableCollection<TouristExperience> TouristExperiences
        {
            get { return touristExperiences; }
            set
            {
                if (touristExperiences != value)
                {
                    touristExperiences = value;
                    OnPropertyChanged(); 
                }
            }
        }
    }
}
