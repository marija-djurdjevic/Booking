using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.View.TouristView;
using BookingApp.WPF.Views.TouristView;
using GalaSoft.MvvmLight.Messaging;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class ComplexTourRequestsViewModel : BindableBase
    {
        private ComplexTourRequestService complexRequestService;
        private ObservableCollection<Tuple<ComplexTourRequest, string>> complexTourRequests;
        public ObservableCollection<Tuple<ComplexTourRequest, string>> ComplexTourRequests
        {
            get { return complexTourRequests; }
            set
            {
                complexTourRequests = value;
                OnPropertyChanged(nameof(ComplexTourRequests));
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
        private int unreadNotificationCount;
        public int UnreadNotificationCount
        {
            get { return unreadNotificationCount; }
            set
            {
                unreadNotificationCount = value;
                OnPropertyChanged(nameof(UnreadNotificationCount));
            }
        }
        public User LoggedInUser { get; set; }
        private readonly TouristGuideNotificationService notificationService;
        public RelayCommand InboxCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand SortingRequeststCommand { get; set; }
        public RelayCommand CreateCommand { get; set; }
        public RelayCommand ScrollToTopCommand { get; private set; }
        public RelayCommand ScrollToBottomCommand { get; private set; }
        public RelayCommand ScrollDownCommand { get; private set; }
        public RelayCommand ScrollUpCommand { get; private set; }

        public ComplexTourRequestsViewModel(User loggedInUser)
        {
            LoggedInUser = loggedInUser;
            ComplexTourRequests = new ObservableCollection<Tuple<ComplexTourRequest, string>>();
            complexRequestService = new ComplexTourRequestService(Injector.CreateInstance<ITourRequestRepository>(), Injector.CreateInstance<IComplexTourRequestRepository>());
            notificationService = new TouristGuideNotificationService(Injector.CreateInstance<ITouristGuideNotificationRepository>());

            GetMyRequests();
            UnreadNotificationCount = notificationService.GetUnreadNotificationCount(LoggedInUser.Id);

            InboxCommand = new RelayCommand(OpenInbox);
            CreateCommand = new RelayCommand(CreateTourRequest);
            HelpCommand = new RelayCommand(Help);
            SortingRequeststCommand = new RelayCommand(SortingSelectionChanged);
            ScrollToTopCommand = new RelayCommand(ScrollToTop);
            ScrollToBottomCommand = new RelayCommand(ScrollToBottom);
            ScrollDownCommand = new RelayCommand(ScrollDown);
            ScrollUpCommand = new RelayCommand(ScrollUp);
        }
        private void ScrollUp()
        {
            Messenger.Default.Send(new NotificationMessage("ScrollRequestsUp"));
        }

        private void ScrollDown()
        {
            Messenger.Default.Send(new NotificationMessage("ScrollRequestsDown"));
        }

        private void ScrollToBottom()
        {
            Messenger.Default.Send(new NotificationMessage("ScrollRequestsToBottom"));
        }

        private void ScrollToTop()
        {
            Messenger.Default.Send(new NotificationMessage("ScrollRequestsToTop"));
        }
        private void Help()
        {

        }
        private void GetMyRequests()
        {
            ComplexTourRequests.Clear();
            int i = 0;
            string title = "Complex request ";
            foreach (var request in complexRequestService.GetByTouristId(LoggedInUser.Id))
            {
                ComplexTourRequests.Add(new Tuple<ComplexTourRequest, string>(request, title + ++i));
            }
        }

        public void OpenInbox()
        {
            new NotificationsWindow(LoggedInUser).ShowDialog();
            UnreadNotificationCount = notificationService.GetUnreadNotificationCount(LoggedInUser.Id);
        }

        public void SortingSelectionChanged()
        {
            complexRequestService.SortTours(ComplexTourRequests, RequestsSelectedSort);
        }
        public void CreateTourRequest()
        {
            new CreateComplexTourRequestWindow(LoggedInUser).ShowDialog();
            GetMyRequests();
        }
    }
}
