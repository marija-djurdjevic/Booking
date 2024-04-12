using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.UseCases;
using BookingApp.View.TouristView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace BookingApp.ViewModel.TouristViewModel
{
    public class ShowAndSearchToursViewModel: INotifyPropertyChanged
    {
        public static ObservableCollection<TourDto> Tours { get; set; }
        public User LoggedInUser { get; set; }
        public TourDto SelectedTour { get; set; }

        private readonly TourRepository repository;

        private bool _isCancelSearchButtonVisible;

        public ShowAndSearchToursViewModel(User loggedInUser)
        {
            repository = new TourRepository();
            Tours = new ObservableCollection<TourDto>();
            SelectedTour = new TourDto();

            IsCancelSearchButtonVisible = false;
            LoggedInUser = loggedInUser;
            GetAllTours();

        }

        public bool IsCancelSearchButtonVisible
        {
            get => _isCancelSearchButtonVisible;
            set
            {
                if (value != _isCancelSearchButtonVisible)
                {
                    _isCancelSearchButtonVisible = value;
                    OnPropertyChanged("IsCancelSearchButtonVisible");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void GetAllTours()
        {
            Tours.Clear();
            foreach (var tour in repository.GetAll())
            {
                Tours.Add(new TourDto(tour));
            }
        }

        public void SelectedTourCard(object sender)
        {
            Border border = (Border)sender;
            SelectedTour = (TourDto)border.DataContext;
            if (SelectedTour.MaxTouristNumber > 0)
            {
                TourBookingWindow tourBookingWindow = new TourBookingWindow(SelectedTour, LoggedInUser.Id);
                tourBookingWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("The tour is fully booked. Please select an alternative tour from this city.");
                ShowUnbookedToursInCity();
            }
        }

        public void Search()
        {
            SearchWindow searchWindow = new SearchWindow(Tours);
            searchWindow.ShowDialog();

            IsCancelSearchButtonVisible = searchWindow.searchViewModel.IsCancelSearchButtonVisible;
        }

        public void OpenInbox()
        {
            NotificationsWindow notificationsWindow = new NotificationsWindow(LoggedInUser);
            notificationsWindow.ShowDialog();
        }

        public void ShowAllTours()
        {
            IsCancelSearchButtonVisible = false;
            GetAllTours();
        }

        private void ShowUnbookedToursInCity()
        {
            List<Tour> unBookedToursInCity = repository.GetUnBookedToursInCity(SelectedTour.LocationDto.City);

            if (unBookedToursInCity.Count > 0)
            {
                IsCancelSearchButtonVisible = true;
                Tours.Clear();
                foreach (var tour in unBookedToursInCity)
                {
                    Tours.Add(new TourDto(tour));
                }
            }
            else
            {
                MessageBox.Show("There are no tours from that city");
            }
        }

        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
