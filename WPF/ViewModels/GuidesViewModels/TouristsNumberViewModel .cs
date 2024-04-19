using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Navigation;
using BookingApp.View.GuideView;
using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.View;
using BookingApp.Command;
using System.Linq;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Aplication;


namespace BookingApp.WPF.ViewModel.GuidesViewModel
{
    internal class TouristsNumberPageViewModel : BaseViewModel
    {
        private int tourId;
        private int under18Count;
        private int between18And50Count;
        private int over50Count;
        private readonly TouristService touristService;
        private readonly TourReservationService tourReservationService;
        private readonly TouristExperienceService touristExperienceService;
        private RelayCommand navigateHomeCommand;
        private RelayCommand navigateBackCommand;

        public TouristsNumberPageViewModel(int tourId)
        {
            this.tourId = tourId;
            touristExperienceService = new TouristExperienceService(Injector.CreateInstance<ITouristExperienceRepository>());
            touristService = new TouristService(Injector.CreateInstance<ITouristRepository>());
            tourReservationService = new TourReservationService(Injector.CreateInstance<ITourReservationRepository>());
            navigateHomeCommand = new RelayCommand(ExecuteNavigateHome);
            navigateBackCommand = new RelayCommand(ExecuteNavigateBack);
            CountTouristsByAge();
        }

        public int Under18Count
        {
            get { return under18Count; }
            set
            {
                if (under18Count != value)
                {
                    under18Count = value;
                    OnPropertyChanged(nameof(Under18Count));
                }
            }
        }

        public int Between18And50Count
        {
            get { return between18And50Count; }
            set
            {
                if (between18And50Count != value)
                {
                    between18And50Count = value;
                    OnPropertyChanged(nameof(Between18And50Count));
                }
            }
        }

        public int Over50Count
        {
            get { return over50Count; }
            set
            {
                if (over50Count != value)
                {
                    over50Count = value;
                    OnPropertyChanged(nameof(Over50Count));
                }
            }
        }

        private void CountTouristsByAge()
        {

            var users = tourReservationService.GetByTourId(tourId).Where(t => t.IsUser && !string.IsNullOrWhiteSpace(t.JoinedKeyPoint.Name));

            foreach (var user in users)
            {
                int touristId = user.UserId;
                int age = touristService.GetAgeById(touristId);
                if (age < 18)
                {
                    Under18Count++;
                }
                else if (age >= 18 && age <= 50)
                {
                    Between18And50Count++;
                }
                else
                {
                    Over50Count++;
                }
            }
        }
        public RelayCommand NavigateHomeCommand
        {
            get { return navigateHomeCommand; }
            set
            {
                if (navigateHomeCommand != value)
                {
                    navigateHomeCommand = value;
                    OnPropertyChanged();
                }
            }
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

        private void ExecuteNavigateHome()
        {
            var mainPage = new GuideMainPage1();
            GuideMainWindow.MainFrame.Navigate(mainPage);

        }

        private void ExecuteNavigateBack()
        {
            var tourStatisticPage = new TourStatistic();
            GuideMainWindow.MainFrame.Navigate(tourStatisticPage);
        }
    }
}
