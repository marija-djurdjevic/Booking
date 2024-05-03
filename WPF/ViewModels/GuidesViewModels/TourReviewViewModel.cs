using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.View;
using BookingApp.View.GuideView;
using Microsoft.Build.Framework;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.GuidesViewModel
{
    public class TourReviewViewModel : BaseViewModel
    {
        private readonly TouristService touristService;
        private  Tour selectedTour;
        private readonly TouristExperienceService touristExperienceService;
        private readonly TourReservationService tourReservationService;
        private readonly TourService tourService;
        private int tourId;
        private RelayCommand navigateBackCommand;
        private RelayCommand reportReviewCommand;
        private RelayCommand sideMenuCommand;
        private ObservableCollection<TouristExperience> touristExperiences;

        public TourReviewViewModel(int tourId)
        {
            this.tourId = tourId;
            tourService = new TourService(Injector.CreateInstance<ITourRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            SelectedTour = tourService.GetTourById(tourId);
            touristService = new TouristService(Injector.CreateInstance<ITouristRepository>());
            touristExperienceService = new TouristExperienceService(Injector.CreateInstance<ITouristExperienceRepository>());
            tourReservationService = new TourReservationService(Injector.CreateInstance<ITourReservationRepository>());
            navigateBackCommand = new RelayCommand(ExecuteNavigateBack);
            reportReviewCommand = new RelayCommand(ExecuteReportReview);
            sideMenuCommand = new RelayCommand(ExecuteSideMenuClick);
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
            var mainPage = new GuideMainPage();
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


        public RelayCommand SideManuCommand
        {
            get { return sideMenuCommand; }
            set
            {
                if (sideMenuCommand != value)
                {
                    sideMenuCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private void ExecuteSideMenuClick()
        {

            var sideMenuPage = new SideMenuPage();
            GuideMainWindow.MainFrame.Navigate(sideMenuPage);

        }


        public Tour SelectedTour
        {
            get { return selectedTour; }
            set
            {
                if (selectedTour != value)
                {
                    selectedTour = value;
                    OnPropertyChanged(nameof(SelectedTour));
                }
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
