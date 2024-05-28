using BookingApp.Aplication.Dto;
using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.View.TouristView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BookingApp.Aplication.UseCases;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApp.Aplication;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Command;
using GalaSoft.MvvmLight.Messaging;
using BookingApp.WPF.Views.TouristView;

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class TouristsDataViewModel : BindableBase
    {
        public ObservableCollection<Tuple<TourReservationViewModel, string, bool>> Tourists { get; set; }
        public TourDto SelectedTour { get; set; }
        public TourRequestViewModel TourRequestViewModel { get; set; }
        public bool IsRequest { get; set; }
        public Tourist LoggedInTourist { get; set; }
        public TourService TourService { get; set; }
        private readonly TouristService touristService;
        private VoucherService voucherService;
        private TourReservationService reservationDataService;
        private TourRequestService requestService;

        public bool IsComplex { get; set; }
        public ComplexTourRequest ComplexTourRequest { get; set; }

        public string TitleTxt { get; set; }
        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand ScrollToTopCommand { get; private set; }
        public RelayCommand ScrollToBottomCommand { get; private set; }
        public RelayCommand ScrollDownCommand { get; private set; }
        public RelayCommand ScrollUpCommand { get; private set; }
        private bool AreDataSaved;

        public TouristsDataViewModel(int touristNumber, TourDto selectedTour, int userId, bool isRequest, TourRequestViewModel tourRequest, bool isComplex, ComplexTourRequest complexTourRequest)
        {
            Tourists = new ObservableCollection<Tuple<TourReservationViewModel, string, bool>>();
            touristService = new TouristService(Injector.CreateInstance<ITouristRepository>(), Injector.CreateInstance<ITouristGuideNotificationRepository>(), Injector.CreateInstance<IVoucherRepository>());
            voucherService = new VoucherService(Injector.CreateInstance<IVoucherRepository>());
            reservationDataService = new TourReservationService(Injector.CreateInstance<ITourReservationRepository>());
            TourService = new TourService(Injector.CreateInstance<ITourRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            requestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), Injector.CreateInstance<ITourRepository>());

            SelectedTour = selectedTour;
            TourRequestViewModel = tourRequest;
            LoggedInTourist = touristService.GetByUserId(userId);
            AreDataSaved = false;
            IsComplex = isComplex;
            ComplexTourRequest = complexTourRequest;

            TitleTxt = "Enter the data of " + touristNumber + " people";

            Tourists.Add(new Tuple<TourReservationViewModel, string, bool>(new TourReservationViewModel(new TourReservation(SelectedTour.Id, LoggedInTourist, true)), "Tourist 1", true));
            for (int i = 0; i < touristNumber - 1; i++)
            {
                int a = i + 2;
                Tourists.Add(new Tuple<TourReservationViewModel, string, bool>(new TourReservationViewModel(new TourReservation(SelectedTour.Id, userId, false)), "Tourist " + a, false));
            }
            IsRequest = isRequest;

            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(CloseWindow);
            HelpCommand = new RelayCommand(Help);
            ScrollToTopCommand = new RelayCommand(ScrollToTop);
            ScrollToBottomCommand = new RelayCommand(ScrollToBottom);
            ScrollDownCommand = new RelayCommand(ScrollDown);
            ScrollUpCommand = new RelayCommand(ScrollUp);
            Messenger.Default.Register<NotificationMessage>(this, SaveReservation);
        }
        private void ScrollUp()
        {
            Messenger.Default.Send(new NotificationMessage("ScrollDataUp"));
        }

        private void ScrollDown()
        {
            Messenger.Default.Send(new NotificationMessage("ScrollDataDown"));
        }

        private void ScrollToBottom()
        {
            Messenger.Default.Send(new NotificationMessage("ScrollDataToBottom"));
        }

        private void ScrollToTop()
        {
            Messenger.Default.Send(new NotificationMessage("ScrollDataToTop"));
        }

        private void SaveReservation(NotificationMessage message)
        {
            if (message.Notification == "SaveReservations")
            {
                this.SaveReservation();
            }
        }

        private void Help()
        {
            new HelpTouristsDataWindow().Show();
        }

        private void CloseWindow()
        {
            // Slanje poruke za zatvaranje prozora koristeći MVVM Light Messaging
            Style style = Application.Current.FindResource("MessageStyle") as Style;
            MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("Are you sure you want to close window?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning, style);
            if (result == MessageBoxResult.Yes)
                Messenger.Default.Send(new NotificationMessage("CloseTouristsDataWindowMessage"));
        }

        public void Confirm()
        {
            foreach (Tuple<TourReservationViewModel, string, bool> data in Tourists)
            {
                if (!data.Item1.IsValid)
                {
                    Style style = Application.Current.FindResource("MessageStyle") as Style;
                    MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("All fields must be filled correctly!", "Error", MessageBoxButton.OK, MessageBoxImage.Error, style);
                    return;
                }
            }
            if (IsRequest)
            {
                ConfirmRequestCreation();
            }
            else
            {
                UseVouchers();
            }
        }

        public void ConfirmRequestCreation()
        {
            foreach (Tuple<TourReservationViewModel, string, bool> data in Tourists)
            {
                Tuple<string, string, int> person = new Tuple<string, string, int>(data.Item1.TouristFirstName, data.Item1.TouristLastName, data.Item1.TouristAge);
                TourRequestViewModel.Persons.Add(person);
            }
            if (!IsComplex)
            {
                TourRequestViewModel.ComplexId = -1;
                requestService.CreateRequest(TourRequestViewModel.ToTourRequest());
                Style style = Application.Current.FindResource("MessageStyle") as Style;
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("Tour request successfully created!", "Request", MessageBoxButton.OK, MessageBoxImage.Information, style);
            }
            else
            {
                ComplexTourRequest.TourRequests.Add(TourRequestViewModel.ToTourRequest());
            }
            Messenger.Default.Send(new NotificationMessage("CloseTouristsDataWindowMessage"));
            Messenger.Default.Send(new NotificationMessage("CloseCreateTourRequestWindowMessage"));
        }

        public void UseVouchers()
        {

            if (voucherService.GetByToueristId(LoggedInTourist.Id).Count() > 0)
            {
                Style style = Application.Current.FindResource("MessageStyle") as Style;
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("Would you like to use vouchers for booking this tour?", "Vouchers", MessageBoxButton.YesNo, MessageBoxImage.Information, style);
                if (result == MessageBoxResult.Yes)
                {
                    new VouchersForReservationWindow(LoggedInTourist).ShowDialog();
                }
                else
                {
                    SaveReservation();
                }

            }
            else
            {
                SaveReservation();
            }
        }

        private void SaveReservation()
        {
            if (AreDataSaved)
                return;
            foreach (TourReservationViewModel data in Tourists.Select(t => t.Item1).ToList())
            {
                reservationDataService.Save(data.ToTourReservation());
            }

            SelectedTour.MaxTouristNumber -= Tourists.Count();
            TourService.Update(SelectedTour.ToTour());
            AreDataSaved = true;
            Style style = Application.Current.FindResource("MessageStyle") as Style;
            MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("Reservation successfully created!", "Reservation", MessageBoxButton.OK, MessageBoxImage.Information, style);
            Messenger.Default.Send(new NotificationMessage("CloseTouristsDataWindowMessage"));
            Messenger.Default.Send(new NotificationMessage("CloseTourBookingWindowMessage"));
        }
    }
}
