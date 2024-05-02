using BookingApp.Aplication.UseCases;
using BookingApp.Aplication;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.View.TouristView;
using BookingApp.WPF.Views.TouristView;
using GalaSoft.MvvmLight.Messaging;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Command;
using System.Windows;

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class CreateComplexTourRequestViewModel : BindableBase
    {
        private readonly ComplexTourRequestService complexTourRequestService;

        private ObservableCollection<Tuple<TourRequestViewModel, string>> tourRequests;
        public ObservableCollection<Tuple<TourRequestViewModel, string>> TourRequests
        {
            get { return tourRequests; }
            set
            {
                tourRequests = value;
                OnPropertyChanged(nameof(TourRequests));
            }
        }

        private ComplexTourRequest complexTourRequest;
        public ComplexTourRequest ComplexTourRequest
        {
            get { return complexTourRequest; }
            set
            {
                complexTourRequest = value;
                OnPropertyChanged(nameof(ComplexTourRequest));
            }
        }
        public User LoggedInUser { get; set; }
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand<object> RemoveCommand { get; set; }
        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand CreateCommand { get; set; }
        public RelayCommand ScrollToTopCommand { get; private set; }
        public RelayCommand ScrollToBottomCommand { get; private set; }
        public RelayCommand ScrollDownCommand { get; private set; }
        public RelayCommand ScrollUpCommand { get; private set; }

        public CreateComplexTourRequestViewModel(User loggedInUser)
        {
            LoggedInUser = loggedInUser;
            TourRequests = new ObservableCollection<Tuple<TourRequestViewModel, string>>();
            complexTourRequestService = new ComplexTourRequestService(Injector.CreateInstance<ITourRequestRepository>(), Injector.CreateInstance<IComplexTourRequestRepository>());
            ComplexTourRequest = new ComplexTourRequest();
            ComplexTourRequest.TouristId = loggedInUser.Id;
            GetMyRequests();

            CreateCommand = new RelayCommand(AddPart);
            HelpCommand = new RelayCommand(Help);
            ScrollToTopCommand = new RelayCommand(ScrollToTop);
            ScrollToBottomCommand = new RelayCommand(ScrollToBottom);
            ScrollDownCommand = new RelayCommand(ScrollDown);
            ScrollUpCommand = new RelayCommand(ScrollUp);
            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(CloseWindow);
            RemoveCommand = new RelayCommand<object>(RemovePart);
            AddPart();
        }

        private void RemovePart(object obj)
        {
            Tuple<TourRequestViewModel, string> tourRequests = obj as Tuple<TourRequestViewModel, string>;
            if (tourRequests != null)
            {
                ComplexTourRequest.TourRequests.RemoveAll(t => t.Id == tourRequests.Item1.Id);
                GetMyRequests();
            }
            else
            {
                Style style = Application.Current.FindResource("MessageStyle") as Style;
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("You must select a part to remove", "Error", MessageBoxButton.OK, MessageBoxImage.Warning, style);
            }
        }

        private void CloseWindow()
        {
            // Slanje poruke za zatvaranje prozora koristeći MVVM Light Messaging
            Style style = Application.Current.FindResource("MessageStyle") as Style;
            MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("Are you sure you want to close window?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning, style);
            if (result == MessageBoxResult.Yes)
                Messenger.Default.Send(new NotificationMessage("CloseCreateComplexTourRequestWindowMessage"));
        }
        private void Confirm()
        {
            Style style = Application.Current.FindResource("MessageStyle") as Style;
            if(ComplexTourRequest.TourRequests.Count < 2)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("You must add at least two parts to create complex request.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning, style);
                return;
            }

            MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("Are you sure you want to finish and create complex request?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning, style);
            if (result == MessageBoxResult.Yes)
            {
                complexTourRequestService.CreateComplexRequest(ComplexTourRequest);
                Messenger.Default.Send(new NotificationMessage("CloseCreateComplexTourRequestWindowMessage"));
            }
        }
        private void ScrollUp()
        {
            Messenger.Default.Send(new NotificationMessage("ScrollComplexRequestsUp"));
        }

        private void ScrollDown()
        {
            Messenger.Default.Send(new NotificationMessage("ScrollComplexRequestsDown"));
        }

        private void ScrollToBottom()
        {
            Messenger.Default.Send(new NotificationMessage("ScrollComplexRequestsToBottom"));
        }

        private void ScrollToTop()
        {
            Messenger.Default.Send(new NotificationMessage("ScrollComplexRequestsToTop"));
        }
        private void Help()
        {

        }
        private void GetMyRequests()
        {
            TourRequests.Clear();
            int i = 0;
            string title = "Part ";
            foreach (var request in ComplexTourRequest.TourRequests)
            {
                request.Id = i;
                TourRequests.Add(new Tuple<TourRequestViewModel, string>(new TourRequestViewModel(request), title + ++i));
            }
        }
        public void AddPart()
        {
            new CreateTourRequestWindow(LoggedInUser, true, ComplexTourRequest).ShowDialog();
            GetMyRequests();
        }
    }
}
