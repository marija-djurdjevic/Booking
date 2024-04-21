using BookingApp.Aplication;
using BookingApp.Aplication.Dto;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.View.TouristView;
using BookingApp.WPF.Views.TouristView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class TourRequestsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly TourRequestService tourRequestService;

        private ObservableCollection<Tuple<TourRequest, string>> tourRequests;
        public ObservableCollection<Tuple<TourRequest, string>> TourRequests
        {
            get { return tourRequests; }
            set
            {
                tourRequests = value;
                OnPropertyChanged(nameof(TourRequests));
            }
        }
        private string requestsSelectedSort;
        public string RequestsSelectedSort
        {
            get { return requestsSelectedSort; }
            set
            {
                requestsSelectedSort = value;
                OnPropertyChanged(nameof(RequestsSelectedSort));
            }
        }
        public User LoggedInUser { get; set; }

        public TourRequestsViewModel(User loggedInUser)
        {
            LoggedInUser = loggedInUser;
            TourRequests = new ObservableCollection<Tuple<TourRequest, string>>();
            tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>());

            GetMyRequests();
        }

        private void GetMyRequests()
        {
            TourRequests.Clear();
            int i = 0;
            string title = "Request ";
            foreach (var request in tourRequestService.GetByTouristId(LoggedInUser.Id))
            {
                TourRequests.Add(new Tuple<TourRequest, string>(request,title + ++i));
            }
        }

        public void OpenInbox()
        {
            new NotificationsWindow(LoggedInUser).ShowDialog();
        }

        public void SortingSelectionChanged()
        {
            tourRequestService.SortTours(TourRequests, RequestsSelectedSort);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

        public void CreateTourRequest()
        {
            new CreateTourRequestWindow(LoggedInUser).ShowDialog();
            GetMyRequests();
        }
    }
}
